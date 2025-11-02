using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

using static MazeSolverVisualizer.DataVisualizer;
using static MazeSolverVisualizer.DataMaze;
using static MazeSolverVisualizer.MainWindow;
using static MazeSolverVisualizer.DataCpp;


namespace MazeSolverVisualizer {
    public class Visualizer {

        //Visualizer
        public async Task UpdateVisualizerCordsBatch(List<(int, int)> updateCords, Color color) {
            if (!playAlgorithmAnimation) 
                return;

            foreach (var (y, x) in updateCords) {
                await UpdateVisualizerAtCoords((y, x), color);
            }

            updateCords.Clear();
        }

        public async Task UpdateVisualizerAtCoords((int Y, int X) update, Color color) {
            
            if (!playAlgorithmAnimation) //put the check here so i dont have to write it 1000x
                return;

            if(runCppAlgorithm && currVisualizingFinalPath && cppFinalPathList.Count > 0) {
                if (cppFinalPathList[finalPathCellsVisualized] == update)
                    UpdatePixels(update.Y, update.X, bothSolversFinalPathCol); //change color to show C++/C# match cells
                else {
                    UpdatePixels(update.Y, update.X, color);                                                                                          //display both paths 
                    UpdatePixels(cppFinalPathList[finalPathCellsVisualized].y, cppFinalPathList[finalPathCellsVisualized].x, cppSolverFinalPathCol);  //individually
                }

                finalPathCellsVisualized++; //both paths are always the same length even if they dont match cells, so we do this to keep track of where we are in the cpp path
            }
            else if (highlightCurrentPos) {
                UpdatePixels(update.Y, update.X, solverPosHighlightCol); //current pos highlight

                if(posHighlighted)
                    UpdatePixels(highlightedPos.y, highlightedPos.x, highlightedPos.c);

                (highlightedPos.y, highlightedPos.x, highlightedPos.c) = (update.Y, update.X, color);
                posHighlighted = true;
            }
            else
                UpdatePixels(update.Y, update.X, color);

            if (update.Y == finishY && update.X == finishX)
                currVisualizingFinalPath = true; 

            animationRuns++;
            if (animationSpeed == 0 ||
                animationRuns >= animationSpeed && playAlgorithmAnimation) {
                animationRuns = 0;
                await Task.Delay(1);
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

        Color GetColorForChar(char c, bool solvPrintIsFinalPathCol) => c switch {
            wallPrint => wallCol,
            solverPrint => solvPrintIsFinalPathCol ? csSolverFinalPathCol : solverCol,
            _ => backgroundCol
        };

        public void CreateOrUpdateVisualizer(bool visualizeCppPath = true, bool solvPrintIsFinalPathCol = true) {

            //Visualizer size parse
            if (int.TryParse(_mainWindow.GUI_visualizerCellSize.Text, out int parse)) {
                cellSize = parse;
                _mainWindow.GUI_visualizer.Width = mazeSize * cellSize;
                _mainWindow.GUI_visualizer.Height = mazeSize * cellSize;
            }
            else {
                _mainWindow.GUI_visualizerCellSize.Text = cellSize.ToString();
            }

            //create new if size changed or == null
            if (visualBitmap == null || visualBitmap.PixelWidth != mazeSize * cellSize) {
                visualBitmap = new WriteableBitmap(
                      mazeSize * cellSize, mazeSize * cellSize, 96, 96, PixelFormats.Bgra32, null
                );

                pixelArray = new byte[visualBitmap.PixelWidth * visualBitmap.PixelHeight * bytesPerPixel];
                _mainWindow.GUI_visualizerBitmap.Source = visualBitmap;
            }

            //update whole visualizer
            for (int y = 0; y < mazeSize; y++) {
                for (int x = 0; x < mazeSize; x++) {
                    Color color = GetColorForChar(maze[y, x], solvPrintIsFinalPathCol);

                    if(visualizeCppPath && cppFinalPathHashSet.Count > 0 && cppFinalPathHashSet.Contains((y, x))) {
                        if (color == csSolverFinalPathCol)
                            color = bothSolversFinalPathCol;
                        else
                            color = cppSolverFinalPathCol;
                    }

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
