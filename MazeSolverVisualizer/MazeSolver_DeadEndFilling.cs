using System.Windows.Media;

using static MazeSolverVisualizer.Utils; 
using static MazeSolverVisualizer.Data;


namespace MazeSolverVisualizer {
    public class MazeSolver_DeadEndFilling {

        //main
        public static async Task CallSolver_DeadEndFilling() => await _deadEndFill.Loop();

        async Task Loop() {
            while (RunLoop_DeadEndFill()) {
                SolveLogic();

                if (playSolveAnimation)
                    await _utils.EasyVisUpdateListManager(visUpdateCords, Colors.Green);
            }

            await BacktrackBestPath_DeadEndFill();

            if (!playSolveAnimation)
                _utils.CreateOrUpdateVisualizer();

        }


        //deep logic
        async Task BacktrackBestPath_DeadEndFill() {
            visUpdateCords.Add((startY, startX));
            
            for (botY = 1; botY <= mazeSize - 2; botY++) {
                for (botX = 1; botX <= mazeSize - 2; botX++) {
                    if (maze[botY, botX] != freeCellPrint)
                        continue;

                    visUpdateCords.Add((botY, botX));
                }
            }

            visUpdateCords.Add((mazeSize - 2, mazeSize - 1));
            await _utils.EasyVisUpdateListManager(visUpdateCords, Colors.Red);
        }

        void SolveLogic() {
            cellsBlockedThisRound = 0;
            for (botY = 1; botY <= mazeSize - 2; botY++) {
                for (botX = 1; botX <= mazeSize - 2; botX++) {
                    if (maze[botY, botX] != freeCellPrint)
                        continue;

                    int validDirNum = 4;

                    if (maze[botY - 1, botX] != freeCellPrint)
                        validDirNum--;

                    if (maze[botY + 1, botX] != freeCellPrint)
                        validDirNum--;

                    if (maze[botY, botX - 1] != freeCellPrint)
                        validDirNum--;

                    if (maze[botY, botX + 1] != freeCellPrint)
                        validDirNum--;

                    if(validDirNum == 1) {
                        visUpdateCords.Add((botY, botX));
                        moveHistory.Add((botY, botX));
                        maze[botY, botX] = solverPrint;
                        cellsBlockedThisRound++;
                    }
                }
            }
        }
    }
}