using System.Windows;
using System.Windows.Media;

using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.DataGlobal;
using static MazeSolverVisualizer.DataRandomMove;


namespace MazeSolverVisualizer {
    public class MazeSolver_Random {

        //main 
        public static async Task CallSolver_Random() => await _random.Loop();

        async Task Loop() {

            while (RunLoop_Solver()) {
                MoveBot(GetMoveDirection(), ref botY, ref botX);

                maze[botY, botX] = solverPrint;

                await _visl.UpdateVisualizerAtCoords((botY, botX), Colors.Green);
            }

            if (!playAlgorithmAnimation)
                _visl.CreateOrUpdateVisualizer();

            DataRandomMove.Reset();
        }

        //deep logic
        MoveDirections GetMoveDirection() {
            validDir = Enum.GetValues(typeof(MoveDirections)).Cast<MoveDirections>().ToList();

            if (botX == 0 || maze[botY, botX - 1] == wallPrint)
                validDir.Remove(MoveDirections.Left);

            if (botX == mazeSize - 1 || maze[botY, botX + 1] == wallPrint) 
                validDir.Remove(MoveDirections.Right);
            
            if (botY == 0 || maze[botY - 1, botX] == wallPrint) 
                validDir.Remove(MoveDirections.Up);
            
            if (botY == mazeSize - 1 || maze[botY + 1, botX] == wallPrint) 
                validDir.Remove(MoveDirections.Down);
            
            return validDir[rndm.Next(validDir.Count)];
        }
    }
}
