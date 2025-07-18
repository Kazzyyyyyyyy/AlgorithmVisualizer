using System.Windows.Media;

using static MazeSolverVisualizer.Data;
using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.MainWindow;


namespace MazeSolverVisualizer {
    public class MazeGenerator {

        //main
        public static void CallGenerator() {

            //GUI parse
            if (int.TryParse(_mainWindow.GUI_mazeSize.Text, out int parse) && parse != mazeSize) {
                mazeSize = parse;
                maze = new char[mazeSize, mazeSize];
            }
            else 
                _mainWindow.GUI_mazeSize.Text = mazeSize.ToString();

            if (updateForCleanMazeVisual.Count > 0)
                updateForCleanMazeVisual.Clear();


            _mazeGenerator.Loop();
        }

        async void Loop() {
            _utils.GenMazeArray();

            //start prepare
            botX = 1; //dont let the gen start on the border (0) bc he only checks if he CAN go there 
                      //but if hes already there he can move freely and destroy the border
            moveHistory.Add((botY, botX));
            maze[botY, botX] = genPrint;

            if(playSolveAnimation)
                _utils.CreateOrUpdateVisualizer();


            while (RunLoop_Generator()) {
                MoveDirections? currDir = GetMoveDirection();

                if (currDir == null) {
                    _utils.Backtrack();
                    if (playSolveAnimation)
                        await _utils.EasyVisUpdateListManager(visUpdateCords, (Color)ColorConverter.ConvertFromString("#3C3C3C"));

                    continue;
                }
                //else if (moveHistory.Count == 0)
                //    break;
                

                MoveBot(currDir);

                maze[botY, botX] = genPrint;
                moveHistory.Add((botY, botX));

                if (playSolveAnimation) {
                    visUpdateCords.Add((botY, botX));
                    await _utils.EasyVisUpdateListManager(visUpdateCords, Colors.Gray);
                }
            }

            //open up the maze a little
            if(easyMaze)
                RemoveRandomWalls();

            if (playSolveAnimation)
                await _utils.EasyVisUpdateListManager(visUpdateCords, (Color)ColorConverter.ConvertFromString("#3C3C3C"));
            else
                _utils.CreateOrUpdateVisualizer();
            
            ResetAllVars();
        }

        //deep logic 
        public static void RemoveRandomWalls() {
            for (int y = 1; y < mazeSize - 1; y++) {
                for (int x = 1; x < mazeSize - 1; x++) {
                    if (maze[y, x] == 'X') {
                        if (rndm.Next(50) == 1) {
                            visUpdateCords.Add((y, x));
                            maze[y, x] = ' ';
                        }
                    }
                }
            }
        }

        MoveDirections? GetMoveDirection() {
            validDir = Enum.GetValues(typeof(MoveDirections)).Cast<MoveDirections>().ToList();

            //border check
            if (botX <= 1)
                validDir.Remove(MoveDirections.Left);

            if (botX >= mazeSize - 2)
                validDir.Remove(MoveDirections.Right);

            if (botY <= 1)
                validDir.Remove(MoveDirections.Up);

            if (botY >= mazeSize - 2)
                validDir.Remove(MoveDirections.Down);


            //path behind wall check (dont want to many connected paths or maze will be easy)
            if (botX >= 2 && maze[botY, botX - 2] == genPrint)
                validDir.Remove(MoveDirections.Left);

            if (botX <= mazeSize - 3 && maze[botY, botX + 2] == genPrint)
                validDir.Remove(MoveDirections.Right);

            if (botY >= 2 && maze[botY - 2, botX] == genPrint)
                validDir.Remove(MoveDirections.Up);

            if (botY <= mazeSize - 3 && maze[botY + 2, botX] == genPrint)
                validDir.Remove(MoveDirections.Down);


            //gen sends himself to the cells around him and checks if they are viable to go there
            List<MoveDirections> testValidDir = new(validDir);

            foreach (MoveDirections moveDir in testValidDir) {
                int botYTest = botY, botXTest = botX;

                switch (moveDir) {
                    case MoveDirections.Up:
                        botYTest -= 1;

                        if (botXTest != 0 && maze[botYTest, botXTest - 1] == genPrint)
                            validDir.Remove(MoveDirections.Up);

                        if (botXTest != mazeSize - 1 && maze[botYTest, botXTest + 1] == genPrint)
                            validDir.Remove(MoveDirections.Up);
                        break;

                    case MoveDirections.Down:
                        botYTest += 1;

                        if (botXTest != 0 && maze[botYTest, botXTest - 1] == genPrint)
                            validDir.Remove(MoveDirections.Down);

                        if (botXTest != mazeSize - 1 && maze[botYTest, botXTest + 1] == genPrint)
                            validDir.Remove(MoveDirections.Down);
                        break;

                    case MoveDirections.Left:
                        botXTest -= 1;

                        if (botYTest != 0 && maze[botYTest - 1, botXTest] == genPrint)
                            validDir.Remove(MoveDirections.Left);

                        if (botYTest != mazeSize - 1 && maze[botYTest + 1, botXTest] == genPrint)
                            validDir.Remove(MoveDirections.Left);
                        break;

                    case MoveDirections.Right:
                        botXTest += 1;

                        if (botYTest != 0 && maze[botYTest - 1, botXTest] == genPrint)
                            validDir.Remove(MoveDirections.Right);

                        if (botYTest != mazeSize - 1 && maze[botYTest + 1, botXTest] == genPrint)
                            validDir.Remove(MoveDirections.Right);
                        break;
                }
            }

            //make sure gen reaches exit
            if ((botY == mazeSize - 2 && botX == mazeSize - 3 || botY == mazeSize - 2 && botX == mazeSize - 4) 
                && !endReached)
                return MoveDirections.Right;

            else if ((botY == mazeSize - 3 && botX == mazeSize - 2 || botY == mazeSize - 4 && botX == mazeSize - 2)
                && !endReached)
                return MoveDirections.Down;


            if (validDir.Count == 0) 
                return null;

            return validDir[rndm.Next(validDir.Count)];
        }
    }
}