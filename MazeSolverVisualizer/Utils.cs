using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using static MazeSolverVisualizer.DataGlobal;
using static MazeSolverVisualizer.MainWindow;
using System.IO;
using System.Windows.Data;

namespace MazeSolverVisualizer {
    public class Utils {

        //generall
        public static void ResetGlobalVars() {
            visualizerUpdateCords.Clear();
            timer.Reset();
            erasedMarkedCells = 0;
            finalPathLength = 0; 

            DataVisualizer.animationRuns = 0;
        }

        public static void MoveBot(MoveDirections? moveDir, ref int botY, ref int botX) {
            switch (moveDir) {
                case MoveDirections.Up:
                    botY -= 1;
                    break;

                case MoveDirections.Down:
                    botY += 1;
                    break;

                case MoveDirections.Left:
                    botX -= 1;
                    break;

                case MoveDirections.Right:
                    botX += 1;
                    break;
            }
        }

        public void Backtrack(List<(int y, int x)> moveHistory,ref int botY, ref int botX) {
            botY = moveHistory.Last().y;
            botX = moveHistory.Last().x;

            moveHistory.RemoveAt(moveHistory.Count - 1);
        }


        //generator
        public static bool RunLoop_Generator() {
            if (DataGenerator.botY == finishY && DataGenerator.botX == finishX - 1) 
                DataGenerator.endReached = true;

            if (DataGenerator.endReached && DataGenerator.moveHistory.Count == 1) 
                return false;
            
            return true;
        }


        //solver
        public static bool RunLoop_DeadEndFill() {
            if (DataDeadEndFill.cellsBlockedThisRound == 0)
                return false; 

            return true;
        } 

        public static bool RunLoop_Solver() {
            if (maze[finishY, finishX] == solverPrint)
                return false;

            return true;
        }

        public void CleanupNotFinalPathMarks(List<(int, int)> finalPath) {
            for (int y = 0; y < mazeSize; y++) {
                for (int x = 0; x < mazeSize; x++) {
                    if (maze[y, x] == solverPrint && !finalPath.Contains((y, x))) {
                        maze[y, x] = freeCellPrint;
                        erasedMarkedCells++; 
                    }
                }
            }
        }


        //Maze array management
        public void FillMazeArray() {
            for (int height = 0; height < mazeSize; height++) {
                for (int width = 0; width < mazeSize; width++) {
                    maze[height, width] = wallPrint;
                }
            }

            maze[startY, startX] = freeCellPrint; //start
            maze[finishY, finishX] = freeCellPrint; //end
        }

        public void CleanupMazeArray() {
            for (int y = 0; y < mazeSize; y++) {
                for (int x = 0; x < mazeSize; x++) {
                    if (maze[y, x] == solverPrint) {
                        maze[y, x] = freeCellPrint;
                    }
                }
            }
        }


        //GUI 
        public static void OutPutDataAfterRun() {
            timer.Stop();
            
            int visitedCells = 0;
            foreach (char c in maze) {
                if (c == solverPrint)
                    visitedCells++;
            }

            _mainWindow.GUI_outPut.Text = playAlgorithmAnimation ? $"{timer.ElapsedMilliseconds}ms (solve + animation)" : $"{timer.ElapsedMilliseconds}ms";
            _mainWindow.GUI_outPut.Text += mazeCurrentlySolved ? $"\n{erasedMarkedCells + visitedCells}" : "";
            _mainWindow.GUI_outPut.Text += mazeCurrentlySolved && !playAlgorithmAnimation ? $" ({visitedCells})" : "";
            _mainWindow.GUI_outPut.Text += mazeCurrentlySolved && playAlgorithmAnimation ? $" ({finalPathLength})" : "";
            _mainWindow.GUI_outPut.Text += mazeCurrentlySolved ? $" visited cells" : "";

            timer.Reset();
        }

        public void DisAndEnableControls() {
            foreach(UIElement el in _mainWindow.GUI_controls.Children) {
                if (el == _mainWindow.GUI_animationSpeed)
                    continue; 

                if (el.IsEnabled)
                    el.IsEnabled = false; 
                else 
                    el.IsEnabled = true;
            }
        }
    }
}
