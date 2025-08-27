using System.Windows.Media;
using static MazeSolverVisualizer.DataGlobal;
using static MazeSolverVisualizer.DataAStar;
using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.MainWindow;


namespace MazeSolverVisualizer {
    public class MazeSolver_A_Star {

        //main
        public static async Task CallSolver_A_Star() {

            //a* greed parse
            if (int.TryParse(_mainWindow.GUI_AStarGreed.Text, out int parse))
                aStarGreed = parse == 0 ? 1 : parse;
            else
                _mainWindow.GUI_AStarGreed.Text = aStarGreed.ToString();


            timer.Start();
            await _a_Star.Loop();
            timer.Stop();
        }

        async Task Loop() {

            var startNode = new Node(startY, startX, 0, Heuristic(startY, startX));
            openList.Enqueue(startNode, startNode.F);

            while (RunLoop_Solver()) {
                SolveLogic();

                await _visl.UpdateVisualizerAtCoords((current.Y, current.X), Colors.Green);

            }

            while (current != null) {
                visualizerUpdateCords.Add((current.Y, current.X));
                current = current.Parent;
            }

            finalPathLength = visualizerUpdateCords.Count; 
            await _visl.UpdateVisualizerCordsBatch(visualizerUpdateCords, Colors.Red);

            if (!playAlgorithmAnimation) {
                _utils.CleanupNotFinalPathMarks(visualizerUpdateCords);
                _visl.CreateOrUpdateVisualizer();
            }

            DataAStar.Reset();
        }


        //deep logic
        void SolveLogic() {

            current = openList.Dequeue();
            openSet.Remove((current.Y, current.X));
            closedSet.Add((current.Y, current.X));

            maze[current.Y, current.X] = solverPrint;

            foreach (var (ny, nx) in new (int, int)[] {
                    (current.Y - 1, current.X),
                    (current.Y + 1, current.X),
                    (current.Y, current.X - 1),
                    (current.Y, current.X + 1) }) {

                if (ny < 0 || ny >= mazeSize || nx < 0 || nx >= mazeSize || closedSet.Contains((ny, nx)) ||
                    maze[ny, nx] != freeCellPrint)
                    continue;

                int h = Heuristic(ny, nx), 
                    tentativeG = current.G + 1;

                if (openSet.TryGetValue((ny, nx), out Node? existing)) {
                    if (tentativeG < existing.G) {
                        existing.G = tentativeG;
                        existing.Parent = current;
                        openList.Enqueue(existing, existing.F);
                    }
                }
                else {
                    var neighbor = new Node(ny, nx, tentativeG, h, current);
                    openList.Enqueue(neighbor, neighbor.F);
                    openSet[(ny, nx)] = neighbor;
                }
            }
        }

        int Heuristic(int y, int x) {
            return aStarGreed * (Math.Abs(y - finishY) + Math.Abs(x - finishX));
        }
    }
}