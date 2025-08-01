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

            while (RunLoop_BFS()) {
                SolveLogic();

                if (playSolveAnimation)
                    await _utils.EasyVisUpdateListManager(visUpdateCords, Colors.Green);
            }

            await _utils.BacktrackBestPath_moveHistory(moveHistory);

            if (!playSolveAnimation)
                _utils.CreateOrUpdateVisualizer();

            _utils.ReturnToCleanMaze();

            ResetAllVars();
        }


        //deep logic
        void SolveLogic() {
            for (botY = 1; botY <= mazeSize - 2; botY++) {
                for (botX = 1; botX <= mazeSize - 1; botX++) {

                    if (maze[botY, botX] == freeCellPrint && ConnectsToPathAndIsVallid(botY, botX)) {
                        maze[botY, botX] = solverPrint;
                        moveHistory.Add((botY, botX));
                        if(playSolveAnimation) visUpdateCords.Add((botY, botX));
                        continue;
                    }
                }
            }
        }

        bool ConnectsToPathAndIsVallid(int y, int x) {
            if (x != 0 && maze[y, x - 1] == solverPrint
                || x != mazeSize - 1 && maze[y, x + 1] == solverPrint
                || y != 0 && maze[y - 1, x] == solverPrint
                || y != mazeSize - 1 && maze[y + 1, x] == solverPrint) {

                return true;
            }

            return false;
        }
    }
}
