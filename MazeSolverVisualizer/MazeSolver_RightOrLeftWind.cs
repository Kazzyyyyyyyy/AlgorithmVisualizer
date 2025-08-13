using System.Windows;
using System.Windows.Media;

using static MazeSolverVisualizer.DataGlobal;
using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.DataRightOrLeftWind;

namespace MazeSolverVisualizer {
    public class MazeSolver_RightOrLeftWind {
        
        //main
        public static async Task CallSolver_RightOrLeftWind(MoveDirections windDir) => await _dirWind.Loop(windDir); 
        
        async Task Loop(MoveDirections windDir) {

            moveHistory.Add((startY, startX));
            maze[startY, startX] = solverPrint;
            await _visl.UpdateVisualizerAtCoords((startY, startX), Colors.Green);

            while (RunLoop_Solver()) {

                MoveDirections? currDir = GetMoveDirection(windDir);

                while (currDir == null) {
                    _utils.Backtrack(moveHistory, ref botY, ref botX);
                    currDir = GetMoveDirection(windDir);
                }

                MoveBot(currDir, ref botY, ref botX);
                moveHistory.Add((botY, botX));
                maze[botY, botX] = solverPrint;

                await _visl.UpdateVisualizerAtCoords((botY, botX), Colors.Green);
            }

            if (!playAlgorithmAnimation)
                _visl.CreateOrUpdateVisualizer();

            DataRightOrLeftWind.Reset();
        }

        //deep logic
        MoveDirections? GetMoveDirection(MoveDirections windDir) {

            if (windDir == MoveDirections.Right) {
                if (botX != mazeSize - 1 && (maze[botY, botX + 1] == freeCellPrint))
                    return MoveDirections.Right;
                
                else if (botY != mazeSize - 1 && (maze[botY + 1, botX] == freeCellPrint))
                    return MoveDirections.Down;

                else if (botX != 0 && (maze[botY, botX - 1] == freeCellPrint))
                    return MoveDirections.Left;

                else if (botY != 0 && (maze[botY - 1, botX] == freeCellPrint))
                    return MoveDirections.Up;
                
                //backtrack 
                return null; 
            }


            if (botX != 0 && (maze[botY, botX - 1] == freeCellPrint))
                return MoveDirections.Left;
            
            else if (botY != mazeSize - 1 && (maze[botY + 1, botX] == freeCellPrint))
                return MoveDirections.Down;
            
            else if (botX != mazeSize - 1 && (maze[botY, botX + 1] == freeCellPrint))
                return MoveDirections.Right;

            else if (botY != 0 && (maze[botY - 1, botX] == freeCellPrint))
                return MoveDirections.Up;

            //backtrack
            return null;
        }
    }
}
