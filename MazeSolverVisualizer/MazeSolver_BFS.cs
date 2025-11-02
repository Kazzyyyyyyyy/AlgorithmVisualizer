
using System.Windows.Media;

using static MazeSolverVisualizer.DataGeneral;
using static MazeSolverVisualizer.DataBFS;
using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.DataVisualizer;
using static MazeSolverVisualizer.DataMaze;

namespace MazeSolverVisualizer {
    public class MazeSolver_BFS {

        //main 
        public static async Task CallSolver_BFS() 
            => await _bfs.Loop();
        

        async Task Loop() {
            timer.Start();

            queue.Enqueue(new Node(startY, startX));

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
            await _visl.UpdateVisualizerCordsBatch(visualizerUpdateCords, csSolverFinalPathCol);

            if (!playAlgorithmAnimation) {
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
