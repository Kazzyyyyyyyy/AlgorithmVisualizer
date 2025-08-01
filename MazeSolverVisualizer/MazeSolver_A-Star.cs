using System.Windows.Media;
using static MazeSolverVisualizer.Data;
using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.MainWindow;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace MazeSolverVisualizer {
    public class MazeSolver_A_Star {

        //main
        public static void CallSolver_A_Star() => _a_Star.Loop();

        async void Loop() {

            while (RunLoop_Solver()) {

                MoveDirections? currDir = GetMoveDirection();

                while (currDir == null) {
                    _utils.Backtrack();
                    cellsMoved--;
                    currDir = GetMoveDirection();
                }

                moveHistory.Add((botY, botX)); 
                
                MoveBot(currDir);
                maze[botY, botX] = solverPrint;
                cellsMoved++; 

                if (playSolveAnimation) {
                    visUpdateCords.Add((botY, botX));
                    await _utils.EasyVisUpdateListManager(visUpdateCords, Colors.Green);
                }
            }

            moveHistory.Add((botY, botX));
            await _utils.BacktrackBestPath_moveHistory(moveHistory);

            if (!playSolveAnimation)
                _utils.CreateOrUpdateVisualizer();

            _utils.ReturnToCleanMaze();
            ResetAllVars();
        }


        //deep logic
        MoveDirections? GetMoveDirection() {

            foreach (var dir in Enum.GetValues(typeof(MoveDirections)).Cast<MoveDirections>().ToList()) {
                switch (dir) {
                    case MoveDirections.Left:
                        if (botX <= 1 || maze[botY, botX - 1] != freeCellPrint)
                            break;

                        moveValues.Add((MoveDirections.Left, CalculateCellValue(botY, botX - 1)));
                        break;

                    case MoveDirections.Right:
                        if (botX == mazeSize - 1 || maze[botY, botX + 1] != freeCellPrint)
                            break;

                        moveValues.Add((MoveDirections.Right, CalculateCellValue(botY, botX + 1)));
                        break;

                    case MoveDirections.Up:
                        if (botY <= 1 || maze[botY - 1, botX] != freeCellPrint)
                            break;

                        moveValues.Add((MoveDirections.Up, CalculateCellValue(botY - 1, botX)));
                        break;

                    case MoveDirections.Down:
                        if (botY >= mazeSize - 2 || maze[botY + 1, botX] != freeCellPrint)
                            break;

                        moveValues.Add((MoveDirections.Down, CalculateCellValue(botY - 1, botX)));
                        break;
                }
            }

            MoveDirections? bestDir = moveValues.Count == 0 ? null : moveValues[0].Item1;
            long? bestValue = moveValues.Count == 0 ? null : moveValues[0].Item2;

            foreach(var (dir, val) in moveValues) {
                if(val <= bestValue) {
                    bestValue = val;
                    bestDir = dir;
                }
            }

            moveValues.Clear();

            return bestDir;
        }

        long CalculateCellValue(int y, int x) {
            return cellsMoved + ((mazeSize - 2) - y + (mazeSize - 1) - x);
        } 
    }
}
