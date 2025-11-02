using System.Windows.Media;

using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.DataGeneral;
using static MazeSolverVisualizer.DataVisualizer;
using static MazeSolverVisualizer.DataMaze;
using static MazeSolverVisualizer.DataDeadEndFill;


namespace MazeSolverVisualizer {
    public class MazeSolver_DeadEndFilling {

        //main
        public static async Task CallSolver_DeadEndFilling() 
            => await _deadEndFill.Loop();

        async Task Loop() {
            timer.Start();

            GetDeadEnds();

            while (RunLoop_DeadEndFill()) {
                SolveLogic(); 

                await _visl.UpdateVisualizerAtCoords(current, solverCol);
            }
            
            timer.Stop();

            await GetFinalPath();

            if (!playAlgorithmAnimation) {
                bool visualizeCppPath = true, solvPrintIsFinalPathCol = false;
                _visl.CreateOrUpdateVisualizer(visualizeCppPath, solvPrintIsFinalPathCol);
            }

            DataDeadEndFill.Reset();
        }

        //deep logic
        void SolveLogic() {
            current = deadEndQueue.Dequeue();

            foreach (var (ny, nx) in new (int, int)[] {
                    (current.Y - 1, current.X),
                    (current.Y + 1, current.X),
                    (current.Y, current.X - 1),
                    (current.Y, current.X + 1) }) {

                if (nx < 0 || nx >= mazeSize || maze[ny, nx] != freeCellPrint)
                    continue;

                if (IsDeadEnd(ny, nx)) {
                    maze[ny, nx] = solverPrint;
                    deadEndQueue.Enqueue((ny, nx));
                }
            }
        }


        void GetDeadEnds() {
            for (int y = 1; y <= mazeSize - 2; y++) {
                for (int x= 1; x <= mazeSize - 2; x++) {
                    if (maze[y, x] != freeCellPrint)
                        continue;

                    if (IsDeadEnd(y, x)) {
                        maze[y, x] = solverPrint;
                        deadEndQueue.Enqueue((y, x));
                    }
                }
            }
        }

        public bool IsDeadEnd(int y, int x) {
            int waysFound = 4;

            if (maze[y - 1, x] != freeCellPrint)
                waysFound--;

            if (maze[y + 1, x] != freeCellPrint)
                waysFound--;

            if (maze[y, x - 1] != freeCellPrint)
                waysFound--;

            if (maze[y, x + 1] != freeCellPrint)
                waysFound--;

            if (waysFound <= 1) 
                return true;

            return false;
        }

        async Task GetFinalPath() {

            if(DataGenerator.imperfectMaze) { //the cool (and acutal) backtracking doesnt work if imperfectMaze
                                              //so we also need this lame one (DEF IS NOT MEANT TO BE USED ON IMPERFECT MAZES)
                visualizerUpdateCords.Add((finishY, finishX));

                for (int botY = mazeSize - 2; botY >= 1; botY--) {
                    for (int botX = mazeSize - 2; botX >= 1; botX--) {
                        if (maze[botY, botX] != freeCellPrint)
                            continue;

                        visualizerUpdateCords.Add((botY, botX));
                    }
                }

                visualizerUpdateCords.Add((startY, startX));
            }
            else { //real backtracking (looks cool)
                current.Y = finishY;
                current.X = finishX;

                visualizerUpdateCords.Add(current);

                while (current.Y != startY || current.X != startX) {
                    foreach (var (ny, nx) in new (int, int)[] {
                    (current.Y - 1, current.X),
                    (current.Y + 1, current.X),
                    (current.Y, current.X - 1),
                    (current.Y, current.X + 1) }) {

                        if (ny < 0 || nx >= mazeSize || maze[ny, nx] != freeCellPrint
                            || visualizerUpdateCords.Contains((ny, nx)))
                            continue;

                        current.Y = ny;
                        current.X = nx;

                        visualizerUpdateCords.Add((current.Y, current.X));
                        break;
                    }
                }
            }

            finalPathLength = visualizerUpdateCords.Count;
            await _visl.UpdateVisualizerCordsBatch(visualizerUpdateCords, csSolverFinalPathCol);
        }
    }
}