using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

using static MazeSolverVisualizer.DataVisualizer;
using static MazeSolverVisualizer.DataGlobal;
using static MazeSolverVisualizer.MainWindow;


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

            UpdatePixels(update.Y, update.X, color);

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

        Color GetColorForChar(char c) => c switch {
            wallPrint => Colors.White,
            solverPrint => Colors.Green,
            _ => backgroundCol
        };

        public void CreateOrUpdateVisualizer() {

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
                    Color color = GetColorForChar(maze[y, x]);

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
