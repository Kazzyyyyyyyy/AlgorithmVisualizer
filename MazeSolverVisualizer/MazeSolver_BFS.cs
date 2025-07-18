using System.Windows;
using System.Windows.Media;

using static MazeSolverVisualizer.Data;
using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.MainWindow;


namespace MazeSolverVisualizer {
    public class MazeSolver_BFS {

        //main 
        public static void CallSolver_BFS() => _bfs.Loop();

        async void Loop() {

            //start prepare
            maze[botY, botX] = solvPrint;
            moveHistory.Add((botY, botX));
            if(playSolveAnimation) visUpdateCords.Add((botY, botX));


            while (RunLoop_BFS()) {
                SolveLogic();

                if (playSolveAnimation)
                    await _utils.EasyVisUpdateListManager(visUpdateCords, Colors.Green);
            }

            await BacktrackBestPath();

            if (!playSolveAnimation)
                _utils.CreateOrUpdateVisualizer();

            _utils.ReturnToCleanMaze();

            ResetAllVars();
        }


        //deep logic
        async Task BacktrackBestPath() {

            visUpdateCords.Add(moveHistory.Last());
            moveHistory.RemoveAt(moveHistory.Count - 1);

            for (int i = moveHistory.Count - 1; i >= 0; i--) {
                if (moveHistory[i].y == visUpdateCords.Last().y && (moveHistory[i].x == visUpdateCords.Last().x - 1 || moveHistory[i].x == visUpdateCords.Last().x + 1)
                    || moveHistory[i].x == visUpdateCords.Last().x && (moveHistory[i].y == visUpdateCords.Last().y - 1 || moveHistory[i].y == visUpdateCords.Last().y + 1)) {
                    visUpdateCords.Add(moveHistory[i]);
                }
            }

            if(!playSolveAnimation) {
                for (int y = 0; y < mazeSize; y++) {
                    for (int x = 0; x < mazeSize; x++) {
                        if (maze[y, x] == 'G' && !visUpdateCords.Contains((y, x))) {
                            maze[y, x] = ' ';
                        }
                    }
                }
                return;
            }

            await _utils.SyncVisualizer(visUpdateCords, Colors.Red);
        }

        void SolveLogic() {
            char c;
            for (botY = 1; botY <= mazeSize - 2; botY++) {
                for (botX = 1; botX <= mazeSize - 1; botX++) {

                    c = maze[botY, botX];

                    if (c == ' ' && ConnectsToPathAndIsVallid(botY, botX)) {
                        maze[botY, botX] = solvPrint;
                        moveHistory.Add((botY, botX));
                        if(playSolveAnimation) visUpdateCords.Add((botY, botX));
                        continue;
                    }
                }
            }
        }

        bool ConnectsToPathAndIsVallid(int y, int x) {
            if (x != 0 && maze[y, x - 1] == solvPrint
                || x != mazeSize - 1 && maze[y, x + 1] == solvPrint
                || y != 0 && maze[y - 1, x] == solvPrint
                || y != mazeSize - 1 && maze[y + 1, x] == solvPrint) {

                return true;
            }
            return false;
        }
    }
}
