using System.Windows;
using System.Windows.Media;

using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.DataGeneral;
using static MazeSolverVisualizer.DataVisualizer;
using static MazeSolverVisualizer.DataMaze;
using static MazeSolverVisualizer.DataRandomMove;


namespace MazeSolverVisualizer {
    public class MazeSolver_Random {

        //main 
        public static async Task CallSolver_Random()
            => await _random.Loop();
        

        async Task Loop() {
            timer.Start();

            maze[startY, startX] = solverPrint;
            await _visl.UpdateVisualizerAtCoords((startY, startX), solverCol);

            while (RunLoop_Solver()) {
                Directions currDir = GetMoveDirection();

                MoveBot(currDir, ref botY, ref botX);
                maze[botY, botX] = solverPrint;

                await _visl.UpdateVisualizerAtCoords((botY, botX), solverCol);
            }

            timer.Stop();

            if (!playAlgorithmAnimation)
                _visl.CreateOrUpdateVisualizer();

            DataRandomMove.Reset();
        }


        //deep logic
        Directions GetMoveDirection() {
            validDir = Enum.GetValues(typeof(Directions)).Cast<Directions>().ToList();

            if (botX == 0 || maze[botY, botX - 1] == wallPrint)
                validDir.Remove(Directions.Left);

            if (botX == mazeSize - 1 || maze[botY, botX + 1] == wallPrint) 
                validDir.Remove(Directions.Right);
            
            if (botY == 0 || maze[botY - 1, botX] == wallPrint) 
                validDir.Remove(Directions.Up);
            
            if (botY == mazeSize - 1 || maze[botY + 1, botX] == wallPrint) 
                validDir.Remove(Directions.Down);

            return validDir[rndm.Next(validDir.Count)];
        }
    }
}
