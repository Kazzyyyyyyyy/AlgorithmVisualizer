using System.Windows.Media;

using static MazeSolverVisualizer.Data;
using static MazeSolverVisualizer.Utils;

namespace MazeSolverVisualizer {
    public class MazeSolver_BFS {

        //main 
        public static async Task CallSolver_BFS() => await _bfs.Loop();

        async Task Loop() {

            queue.Enqueue(new Node(startY, startX));

            while (RunLoop_Solver()) {
                SolveLogic();

                if (playSolveAnimation) {
                    visUpdateCords.Add((current.Y, current.X));
                    await _utils.EasyVisUpdateListManager(visUpdateCords, Colors.Green);
                }
            }

            while (current != null) {
                visUpdateCords.Add((current.Y, current.X));
                current = current.Parent;
            }

            if (playSolveAnimation)
                await _utils.SyncVisualizer(visUpdateCords, Colors.Red);
            else {
                _utils.EraseExcessSolverPrints(visUpdateCords);
                _utils.CreateOrUpdateVisualizer();
            }
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
