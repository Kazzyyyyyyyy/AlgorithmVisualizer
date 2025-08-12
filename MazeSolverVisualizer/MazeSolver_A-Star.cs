using System.Windows.Media;
using static MazeSolverVisualizer.Data;
using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.MainWindow;


namespace MazeSolverVisualizer {
    public class MazeSolver_A_Star {

        //main
        public static async Task CallSolver_A_Star() {

            if (int.TryParse(_mainWindow.GUI_AStarGreed.Text, out int parse))
                aStarGreed = parse == 0 ? 1 : parse;
            else
                _mainWindow.GUI_animationSleep.Text = aStarGreed.ToString();

            await _a_Star.Loop();
        }

        async Task Loop() {

            //start node
            var startNode = new Node(startY, startX, 0, Heuristic(startY, startX));
            openList.Enqueue(startNode, startNode.F);

            while (RunLoop_Solver()) {
                SolveLogic();

                if (playSolveAnimation) {
                    visUpdateCords.Add((current.Y, current.X));
                    await _utils.EasyVisUpdateListManager(visUpdateCords, Colors.Green);
                }
            }

            //backtrack best path
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

            current = openList.Dequeue();
            openSet.Remove((current.Y, current.X));
            closedSet.Add((current.Y, current.X));

            maze[current.Y, current.X] = solverPrint;

            foreach (var dir in new (int y, int x)[] { new(-1, 0), new(1, 0), new(0, -1), new(0, 1) }) {
                int ny = current.Y + dir.y;
                int nx = current.X + dir.x;

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