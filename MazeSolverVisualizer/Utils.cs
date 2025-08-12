using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using static MazeSolverVisualizer.Data;
using static MazeSolverVisualizer.MainWindow;
using System.Windows.Controls;

namespace MazeSolverVisualizer {
    public class Utils {

        //general
        public static void ResetAllVars() {
            botY = 1; botX = 0;

            animationRoundNum = 0;
            endReached = false;
            moveHistory.Clear();
            visUpdateCords.Clear();
            cellsBlockedThisRound = -1;
            timer.Reset();

            openList.Clear();
            closedSet.Clear();
            openSet.Clear();
            queue.Clear();
            current = null!;

            _utils.BlockControlsWhileRunning();
        }

        public static void MoveBot(MoveDirections? moveDir) {
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

        public void Backtrack() {
            botY = moveHistory.Last().y;
            botX = moveHistory.Last().x;

            visUpdateCords.Add(moveHistory.Last());
            moveHistory.RemoveAt(moveHistory.Count - 1);
        }


        //maze generator
        public static bool RunLoop_Generator() {
            if (botY == finishY && botX == finishX - 1) {
                endReached = true;
            }

            if (endReached && moveHistory.Count == 0)
                return false;

            return true;
        }

        //solver
        public static bool RunLoop_DeadEndFill() {
            if (cellsBlockedThisRound == 0)
                return false; 

            return true;
        } 

        public static bool RunLoop_Solver() {
            if (maze[finishY, finishX] == solverPrint)
                return false;

            return true;
        }

        public void EraseExcessSolverPrints(List<(int, int)> list) {
            for (int y = 0; y < mazeSize; y++) {
                for (int x = 0; x < mazeSize; x++) {
                    if (maze[y, x] == solverPrint && !list.Contains((y, x))) {
                        maze[y, x] = freeCellPrint;
                    }
                }
            }
        }

        public async Task BacktrackBestPath_moveHistory(List<(int y, int x)> moveHistory) {

            visUpdateCords.Add(moveHistory.Last());
            moveHistory.RemoveAt(moveHistory.Count - 1);

            for (int i = moveHistory.Count - 1; i >= 0; i--) {
                if (moveHistory[i].y == visUpdateCords.Last().y && (moveHistory[i].x == visUpdateCords.Last().x - 1 || moveHistory[i].x == visUpdateCords.Last().x + 1)
                    || moveHistory[i].x == visUpdateCords.Last().x && (moveHistory[i].y == visUpdateCords.Last().y - 1 || moveHistory[i].y == visUpdateCords.Last().y + 1)) {
                    visUpdateCords.Add(moveHistory[i]);
                }
            }

            if (!playSolveAnimation)
                EraseExcessSolverPrints(visUpdateCords);

            _mainWindow.GUI_outPut.Text += visUpdateCords.Count.ToString();
            await _utils.SyncVisualizer(visUpdateCords, Colors.Red);
        }


        //Maze array management
        public void GenMazeArray() {
            for (int height = 0; height < mazeSize; height++) {
                for (int width = 0; width < mazeSize; width++) {
                    maze[height, width] = wallPrint;
                }
            }

            maze[1, 0] = freeCellPrint; //start
            maze[mazeSize - 2, mazeSize - 1] = freeCellPrint; //end

            CreateOrUpdateVisualizer();
        }

        public void ReturnToCleanMaze() {
            for (int y = 0; y < mazeSize; y++) {
                for (int x = 0; x < mazeSize; x++) {
                    if (maze[y, x] == solverPrint) {
                        maze[y, x] = freeCellPrint;
                    }
                }
            }
        }


        //GUI 
        public void OutPutAfterSolve() {
            timer.Stop();
            int steps = 0;
            foreach (char c in maze) {
                if (c == solverPrint)
                    steps++;
            }

            _mainWindow.GUI_outPut.Text = $"{timer.ElapsedMilliseconds}ms\n{steps} steps";

            timer.Reset();
        }

        public void BlockControlsWhileRunning() {
            foreach(UIElement el in _mainWindow.GUI_controlls.Children) {
                if (el == _mainWindow.GUI_animationSleep)
                    continue; 

                if (el.IsEnabled)
                    el.IsEnabled = false; 
                else 
                    el.IsEnabled = true;
            }
        }


        //Visualizer
        public async Task EasyVisUpdateListManager(List<(int, int)> updateCords, Color color) {
            await SyncVisualizer(updateCords, color);
            updateCords.Clear();
        }

        public void CleanVisualizer() {
            foreach (var (y, x) in updateForCleanMazeVisual)
                UpdatePixels(y, x, backgroundCol);
        }

        public async Task SyncVisualizer(List<(int y, int x)> update, Color color) {

            foreach (var (y, x) in update) {

                if (!updateForCleanMazeVisual.Contains((y, x)))
                    updateForCleanMazeVisual.Add((y, x));

                UpdatePixels(y, x, color);

                animationRoundNum++;
                if (animationTaskDelayIn == 0
                    || animationRoundNum >= animationTaskDelayIn && playSolveAnimation) {
                    animationRoundNum = 0;
                    await Task.Delay(1);
                }
            }
        }

        void UpdatePixels(int y, int x, Color color) {
            byte[] cellPixels = new byte[cellSize * cellSize * bytesPerPixel];
            for (int ty = 0; ty < cellSize; ty++) {
                for (int tx = 0; tx < cellSize; tx++) {
                    int index = (ty * cellSize + tx) * bytesPerPixel;
                    cellPixels[index + 0] = color.B;
                    cellPixels[index + 1] = color.G;
                    cellPixels[index + 2] = color.R;
                    cellPixels[index + 3] = 255;
                }
            }

            visualBitmap.WritePixels(
                new Int32Rect(x * cellSize, y * cellSize, cellSize, cellSize),
                cellPixels, cellSize * bytesPerPixel, 0
            );
        }

        Color GetColorForChar(char c) =>
                c switch {
                    wallPrint => Colors.White,
                    solverPrint => Colors.Green,
                    'T' => Colors.Orange,
                    _ => backgroundCol
                };

        public void CreateOrUpdateVisualizer() {

            //GUI parse
            if (int.TryParse(_mainWindow.GUI_cellSize.Text, out int parse)) {
                cellSize = parse;
                _mainWindow.GUI_visualizer.Width = mazeSize * cellSize;
                _mainWindow.GUI_visualizer.Height = mazeSize * cellSize;
            }
            else {
                _mainWindow.GUI_cellSize.Text = cellSize.ToString();
            }


            //create new if size changed or == null
            if (visualBitmap == null || visualBitmap.PixelWidth != mazeSize * cellSize) {

                visualBitmap = new WriteableBitmap(
                      mazeSize * cellSize, mazeSize * cellSize, 96, 96, PixelFormats.Bgra32, null
                );

                pixelArray = new byte[visualBitmap.PixelWidth * visualBitmap.PixelHeight * bytesPerPixel];
                _mainWindow.GUI_visualizerImage.Source = visualBitmap;
            }

            for (int y = 0; y < mazeSize; y++) {
                for (int x = 0; x < mazeSize; x++) {
                    Color color = GetColorForChar(maze[y, x]);
                    if (maze[y, x] == solverPrint)
                        updateForCleanMazeVisual.Add((y, x));

                    for (int ty = 0; ty < cellSize; ty++) {
                        for (int tx = 0; tx < cellSize; tx++) {
                            int px = (x * cellSize) + tx;
                            int py = (y * cellSize) + ty;
                            int index = (py * visualBitmap.PixelWidth + px) * bytesPerPixel;

                            pixelArray[index + 0] = color.B;
                            pixelArray[index + 1] = color.G;
                            pixelArray[index + 2] = color.R;
                            pixelArray[index + 3] = 255;
                        }
                    }
                }
            }

            visualBitmap.WritePixels(
                new Int32Rect(0, 0, visualBitmap.PixelWidth, visualBitmap.PixelHeight),
                pixelArray, visualBitmap.PixelWidth * bytesPerPixel, 0
            );
        }
    }
}
