using System.Windows.Media;

using static MazeSolverVisualizer.DataGlobal;
using static MazeSolverVisualizer.DataGenerator;
using static MazeSolverVisualizer.Utils;

namespace MazeSolverVisualizer {
    public class MazeGenerator {

        //main
        public static async Task CallGenerator() => await _mazeGenerator.Loop();

        async Task Loop() {

            moveHistory.Add((botY, botX));
            maze[botY, botX] = freeCellPrint; 

            while (RunLoop_Generator()) {
                await _visl.UpdateVisualizerAtCoords((botY, botX), Colors.Gray);

                MoveDirections? currDir = GetMoveDirection();

                while(currDir == null && moveHistory.Count > 0) {
                    _utils.Backtrack(moveHistory, ref botY, ref botX);

                    await _visl.UpdateVisualizerAtCoords((botY, botX), backgroundCol);
                    currDir = GetMoveDirection();
                }

                MoveBot(currDir, ref botY, ref botX);
                
                moveHistory.Add((botY, botX));
                maze[botY, botX] = freeCellPrint;
            }

            if (!playAlgorithmAnimation)
                _visl.CreateOrUpdateVisualizer();

            DataGenerator.Reset();
        }


        //deep logic
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


            //path behind wall check (dont want too many connected paths or maze will be just open
            //and not really a maze)
            if (!imperfectMaze || !ImperfectMazeRandom()) { //maybe skips this part when easyMaze = true so
                                                  //it opens up the maze a little (perfect => imperfect maze)
                if (botX >= 2 && maze[botY, botX - 2] == freeCellPrint)
                    validDir.Remove(MoveDirections.Left);

                if (botX <= mazeSize - 3 && maze[botY, botX + 2] == freeCellPrint)
                    validDir.Remove(MoveDirections.Right);

                if (botY >= 2 && maze[botY - 2, botX] == freeCellPrint)
                    validDir.Remove(MoveDirections.Up);

                if (botY <= mazeSize - 3 && maze[botY + 2, botX] == freeCellPrint)
                    validDir.Remove(MoveDirections.Down);
            }

            //gen sends himself to the cells around him and checks if they are viable to go there
            List<MoveDirections> testValidDir = new(validDir);

            foreach (MoveDirections moveDir in testValidDir) {
                int botYTest = botY, botXTest = botX;

                switch (moveDir) {
                    case MoveDirections.Up:
                        botYTest -= 1;

                        if (botXTest != 0 && maze[botYTest, botXTest - 1] == freeCellPrint || 
                            botXTest != mazeSize - 1 && maze[botYTest, botXTest + 1] == freeCellPrint)
                            validDir.Remove(MoveDirections.Up);

                        break;

                    case MoveDirections.Down:
                        botYTest += 1;

                        if (botXTest != 0 && maze[botYTest, botXTest - 1] == freeCellPrint ||
                            botXTest != mazeSize - 1 && maze[botYTest, botXTest + 1] == freeCellPrint)
                            validDir.Remove(MoveDirections.Down);

                        break;

                    case MoveDirections.Left:
                        botXTest -= 1;

                        if (botYTest != 0 && maze[botYTest - 1, botXTest] == freeCellPrint ||
                            botYTest != mazeSize - 1 && maze[botYTest + 1, botXTest] == freeCellPrint)
                            validDir.Remove(MoveDirections.Left);

                        break;

                    case MoveDirections.Right:
                        botXTest += 1;

                        if (botYTest != 0 && maze[botYTest - 1, botXTest] == freeCellPrint ||
                            botYTest != mazeSize - 1 && maze[botYTest + 1, botXTest] == freeCellPrint)
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

            //init backtracking
            if (validDir.Count == 0) 
                return null;


            return validDir[rndm.Next(validDir.Count)];
        }

        bool ImperfectMazeRandom() {
            if (rndm.Next(50) == 1) //2% chance to remove wall
                return true;

            return false;
        }

    }
}