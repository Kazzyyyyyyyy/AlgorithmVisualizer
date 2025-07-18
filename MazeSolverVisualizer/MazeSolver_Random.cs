using System.Windows;
using System.Windows.Media;

using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.Data;
using static MazeSolverVisualizer.MainWindow;


namespace MazeSolverVisualizer {
    public class MazeSolver_Random {

        //main 
        public static void CallSolver_Random() => _random.Loop();

        async void Loop() {
            
            //start prepare
            visUpdateCords.Add((botY, botX));
            maze[botY, botX] = solvPrint;


            while (RunLoop_Solver()) {
                MoveBot(GetMoveDirection());

                visUpdateCords.Add((botY, botX));
                maze[botY, botX] = solvPrint;

                if (playSolveAnimation) {
                    await _utils.EasyVisUpdateListManager(visUpdateCords, Colors.Green);
                }
            }

            if(!playSolveAnimation)
                _utils.CreateOrUpdateVisualizer();

            _utils.ReturnToCleanMaze();

            ResetAllVars();
        }

        //deep logic
        MoveDirections GetMoveDirection() {
            validDir = Enum.GetValues(typeof(MoveDirections)).Cast<MoveDirections>().ToList();

            if (botX == 0 || maze[botY, botX - 1] == 'X')
                validDir.Remove(MoveDirections.Left);

            if (botX == mazeSize - 1 || maze[botY, botX + 1] == 'X') 
                validDir.Remove(MoveDirections.Right);
            
            if (botY == 0 || maze[botY - 1, botX] == 'X') 
                validDir.Remove(MoveDirections.Up);
            
            if (botY == mazeSize - 1 || maze[botY + 1, botX] == 'X') 
                validDir.Remove(MoveDirections.Down);
            
            return validDir[rndm.Next(validDir.Count)];
        }
    }
}
