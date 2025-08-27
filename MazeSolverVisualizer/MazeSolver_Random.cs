using System.Windows;
using System.Windows.Media;

using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.DataGlobal;
using static MazeSolverVisualizer.DataRandomMove;
using System.ComponentModel.DataAnnotations;


namespace MazeSolverVisualizer {
    public class MazeSolver_Random {

        //main 
        public static async Task CallSolver_Random() {
            timer.Start();
            await _random.Loop();
            timer.Stop();
        }

        async Task Loop() {
            moveHistory.Add((startY, startX));
            maze[startY, startX] = solverPrint;
            await _visl.UpdateVisualizerAtCoords((startY, startX), Colors.Green);

            while (RunLoop_Solver()) {
                Directions? currDir = GetMoveDirection();

                while (currDir == null) {
                    _utils.Backtrack(moveHistory, ref botY, ref botX);
                    currDir = GetMoveDirection();
                }

                MoveBot(currDir, ref botY, ref botX);
                moveHistory.Add((botY, botX));
                maze[botY, botX] = solverPrint;

                await _visl.UpdateVisualizerAtCoords((botY, botX), Colors.Green);
            }

            if (!playAlgorithmAnimation)
                _visl.CreateOrUpdateVisualizer();

            DataRandomMove.Reset();
        }

        //deep logic
        Directions? GetMoveDirection() {
            validDir = Enum.GetValues(typeof(Directions)).Cast<Directions>().ToList();

            if (botX == 0 || maze[botY, botX - 1] != freeCellPrint)
                validDir.Remove(Directions.Left);

            if (botX == mazeSize - 1 || maze[botY, botX + 1] != freeCellPrint) 
                validDir.Remove(Directions.Right);
            
            if (botY == 0 || maze[botY - 1, botX] != freeCellPrint) 
                validDir.Remove(Directions.Up);
            
            if (botY == mazeSize - 1 || maze[botY + 1, botX] != freeCellPrint) 
                validDir.Remove(Directions.Down);

            if (validDir.Count == 0)
                return null;

            return validDir[rndm.Next(validDir.Count)];
        }
    }
}
