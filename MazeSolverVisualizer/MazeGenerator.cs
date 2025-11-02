using System.Windows.Media;

using static MazeSolverVisualizer.DataGeneral;
using static MazeSolverVisualizer.DataGenerator;
using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.DataMaze;
using static MazeSolverVisualizer.DataVisualizer;

namespace MazeSolverVisualizer {
    public class MazeGenerator {

        //main
        public static async Task CallGenerator() 
            => await _mazeGenerator.Loop();
        
        async Task Loop() {
            timer.Start();

            moveHistory.Add((botY, botX));
            maze[botY, botX] = freeCellPrint; 

            while (RunLoop_Generator()) {
                await _visl.UpdateVisualizerAtCoords((botY, botX), mazeGenBacktrackCol);

                Directions? currDir = GetMoveDirection();

                while(currDir == null && moveHistory.Count > 0) {
                    _utils.Backtrack(moveHistory, ref botY, ref botX);

                    await _visl.UpdateVisualizerAtCoords((botY, botX), backgroundCol);
                    currDir = GetMoveDirection();
                }

                MoveBot(currDir, ref botY, ref botX);
                
                moveHistory.Add((botY, botX));
                maze[botY, botX] = freeCellPrint;
            }

            timer.Stop();


            if (!playAlgorithmAnimation)
                _visl.CreateOrUpdateVisualizer();

            DataGenerator.Reset();
        }


        //deep logic
        Directions? GetMoveDirection() {
            validDir = Enum.GetValues(typeof(Directions)).Cast<Directions>().ToList();

            //border check
            if (botX <= 1)
                validDir.Remove(Directions.Left);

            if (botX >= mazeSize - 2)
                validDir.Remove(Directions.Right);

            if (botY <= 1)
                validDir.Remove(Directions.Up);

            if (botY >= mazeSize - 2)
                validDir.Remove(Directions.Down);


            //path behind wall check (dont want too many connected paths or maze will be just open
            //and not really a maze)
            if (!imperfectMaze || !ImperfectMazeRandom()) { //maybe skips this part when easyMaze = true so
                                                            //it opens up the maze a little (perfect => imperfect maze)
                if (botX >= 2 && maze[botY, botX - 2] == freeCellPrint)
                    validDir.Remove(Directions.Left);

                if (botX <= mazeSize - 3 && maze[botY, botX + 2] == freeCellPrint)
                    validDir.Remove(Directions.Right);

                if (botY >= 2 && maze[botY - 2, botX] == freeCellPrint)
                    validDir.Remove(Directions.Up);

                if (botY <= mazeSize - 3 && maze[botY + 2, botX] == freeCellPrint)
                    validDir.Remove(Directions.Down);
            }

            //gen sends himself to the cells around him and checks if they are viable to go there
            List<Directions> testValidDir = new(validDir);

            foreach (Directions moveDir in testValidDir) {
                int botYTest = botY, botXTest = botX;

                switch (moveDir) {
                    case Directions.Up:
                        botYTest -= 1;

                        if (botXTest != 0 && maze[botYTest, botXTest - 1] == freeCellPrint || 
                            botXTest != mazeSize - 1 && maze[botYTest, botXTest + 1] == freeCellPrint)
                            validDir.Remove(Directions.Up);
                        break;

                    case Directions.Down:
                        botYTest += 1;

                        if (botXTest != 0 && maze[botYTest, botXTest - 1] == freeCellPrint ||
                            botXTest != mazeSize - 1 && maze[botYTest, botXTest + 1] == freeCellPrint)
                            validDir.Remove(Directions.Down);
                        break;

                    case Directions.Left:
                        botXTest -= 1;

                        if (botYTest != 0 && maze[botYTest - 1, botXTest] == freeCellPrint ||
                            botYTest != mazeSize - 1 && maze[botYTest + 1, botXTest] == freeCellPrint)
                            validDir.Remove(Directions.Left);
                        break;

                    case Directions.Right:
                        botXTest += 1;

                        if (botYTest != 0 && maze[botYTest - 1, botXTest] == freeCellPrint ||
                            botYTest != mazeSize - 1 && maze[botYTest + 1, botXTest] == freeCellPrint)
                            validDir.Remove(Directions.Right);
                        break;
                }
            }

            //make sure gen reaches exit
            if(botY == mazeSize - 2 && !endReached) { //y border
                if(finishX - botX <= 4) {
                    return Directions.Right;
                } 
            }

            if (botX == mazeSize - 2 && !endReached) { //x border
                if (finishY - botY <= 4) {
                    return Directions.Down;
                }
            }

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