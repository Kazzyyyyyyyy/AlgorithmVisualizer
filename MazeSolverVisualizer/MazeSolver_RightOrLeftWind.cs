using System.Windows;
using System.Windows.Media;

using static MazeSolverVisualizer.Data;
using static MazeSolverVisualizer.Utils;

namespace MazeSolverVisualizer {
    public class MazeSolver_RightOrLeftWind {
        
        //main
        public static async Task CallSolver_RightOrLeftWind(MoveDirections windDir) => await _dirWind.Loop(windDir); 
        
        async Task Loop(MoveDirections windDir) {

            while (RunLoop_Solver()) {

                MoveDirections? currDir = GetMoveDirection(windDir);

                while (currDir == null) {
                    _utils.Backtrack();
                    currDir = GetMoveDirection(windDir);
                }

                moveHistory.Add((botY, botX));

                MoveBot(currDir);
                maze[botY, botX] = solverPrint;

                if (playSolveAnimation) {
                    visUpdateCords.Add((botY, botX));
                    await _utils.EasyVisUpdateListManager(visUpdateCords, Colors.Green);
                }
            }

            if (!playSolveAnimation)
                _utils.CreateOrUpdateVisualizer();

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
