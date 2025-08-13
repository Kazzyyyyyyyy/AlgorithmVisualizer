using System.Windows.Media;

using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.DataGlobal;
using static MazeSolverVisualizer.DataDeadEndFill;


namespace MazeSolverVisualizer {
    public class MazeSolver_DeadEndFilling {

        //main
        public static async Task CallSolver_DeadEndFilling() => await _deadEndFill.Loop();

        async Task Loop() {
            while (RunLoop_DeadEndFill()) {
                SolveLogic();

                await _visl.UpdateVisualizerCordsBatch(visualizerUpdateCords, Colors.Green);
            }

            await GetFinalPath();

            if (!playAlgorithmAnimation)
                _visl.CreateOrUpdateVisualizer();

            DataDeadEndFill.Reset();
        }


        //deep logic
        async Task GetFinalPath() {
            visualizerUpdateCords.Add((startY, startX));
            
            for (botY = 1; botY <= mazeSize - 2; botY++) {
                for (botX = 1; botX <= mazeSize - 2; botX++) {
                    if (maze[botY, botX] != freeCellPrint)
                        continue;

                    visualizerUpdateCords.Add((botY, botX));
                }
            }

            visualizerUpdateCords.Add((finishY, finishX)); 
            finalPathLength = visualizerUpdateCords.Count;
            
            await _visl.UpdateVisualizerCordsBatch(visualizerUpdateCords, Colors.Red);
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
                        visualizerUpdateCords.Add((botY, botX));
                        maze[botY, botX] = solverPrint;
                        cellsBlockedThisRound++;
                    }
                }
            }
        }
    }
}