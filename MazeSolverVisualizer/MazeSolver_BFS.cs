using System.Windows.Media;

using static MazeSolverVisualizer.DataGlobal;
using static MazeSolverVisualizer.DataBFS;
using static MazeSolverVisualizer.Utils;

namespace MazeSolverVisualizer {
    public class MazeSolver_BFS {

        //main 
        public static async Task CallSolver_BFS() {
            timer.Start();
            await _bfs.Loop();
            timer.Stop();
        }

        async Task Loop() {

            queue.Enqueue(new Node(startY, startX));

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

            if(!playAlgorithmAnimation) {
                _utils.CleanupNotFinalPathMarks(visualizerUpdateCords);
                _visl.CreateOrUpdateVisualizer();
            }

            DataBFS.Reset();
        }


        //deep logic
        void SolveLogic() {

            current = queue.Dequeue();
            maze[current.Y, current.X] = solverPrint;

            foreach (var (ny, nx) in new (int, int)[] {
                    (current.Y - 1, current.X),
                    (current.Y + 1, current.X),
                    (current.Y, current.X - 1),
                    (current.Y, current.X + 1) }) {

                if (nx < 0 || nx >= mazeSize || closedSet.Contains((ny, nx)) || maze[ny, nx] != freeCellPrint)
                    continue;

                queue.Enqueue(new Node(ny, nx, parent: current));
                closedSet.Add((ny, nx));
            }
        }
    }
}
