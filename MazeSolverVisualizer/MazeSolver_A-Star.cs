using System.Windows.Media;
using static MazeSolverVisualizer.DataGeneral;
using static MazeSolverVisualizer.DataAStar;
using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.MainWindow;
using static MazeSolverVisualizer.DataVisualizer;
using static MazeSolverVisualizer.DataMaze;


namespace MazeSolverVisualizer {
    public class MazeSolver_A_Star {

        //main
        public static async Task CallSolver_A_Star() {

            //a* greed parse
            if (int.TryParse(_mainWindow.GUI_AStarGreed.Text, out int parse))
                aStarGreed = parse == 0 ? 1 : parse;
            else
                _mainWindow.GUI_AStarGreed.Text = aStarGreed.ToString();

            await _a_Star.Loop();
        }

        async Task Loop() {

            timer.Start();

            var startNode = new Node(startY, startX, 0, Heuristic(startY, startX));
            openList.Enqueue(startNode, startNode.F);

            while (RunLoop_Solver()) {
                SolveLogic();

                await _visl.UpdateVisualizerAtCoords((current.Y, current.X), solverCol);
            }

            while (current != null) {
                visualizerUpdateCords.Add((current.Y, current.X));
                current = current.Parent;
            }
            
            timer.Stop();

            finalPathLength = visualizerUpdateCords.Count; 

            if (!playAlgorithmAnimation) {
                _utils.CleanupNotFinalPathMarks(visualizerUpdateCords);
                _visl.CreateOrUpdateVisualizer();
            }
            else
                await _visl.UpdateVisualizerCordsBatch(visualizerUpdateCords, csSolverFinalPathCol);

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
                    Node neighbor = new(ny, nx, tentativeG, h, current);
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