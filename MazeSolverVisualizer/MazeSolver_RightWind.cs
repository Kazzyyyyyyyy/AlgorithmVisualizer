using System.Windows;
using System.Windows.Media;

using static MazeSolverVisualizer.Data;
using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.MainWindow;


namespace MazeSolverVisualizer {
    public class MazeSolver_RightWind {
        
        //main
        public static void CallSolver_RightWind() => _rightWind.Loop(); 
        
        async void Loop() {

            while (RunLoop_Solver()) {
                MoveDirections? currDir = GetMoveDirection();

                while (currDir == null) {
                    _utils.Backtrack();
                    currDir = GetMoveDirection();
                }

                moveHistory.Add((botY, botX));

                MoveBot(currDir);
                maze[botY, botX] = solverPrint;

                if (playSolveAnimation) {
                    visUpdateCords.Add((botY, botX));
                    await _utils.EasyVisUpdateListManager(visUpdateCords, Colors.Green);
                }
            }

            if(!playSolveAnimation)
                _utils.CreateOrUpdateVisualizer();

            _utils.ReturnToCleanMaze();

            ResetAllVars();
        }


        //deep logic
        MoveDirections? GetMoveDirection() {
            validDir = Enum.GetValues(typeof(MoveDirections)).Cast<MoveDirections>().ToList();

            //wall check 
            if (botX != 0 && (maze[botY, botX - 1] == wallPrint || maze[botY, botX - 1] == solverPrint))
                validDir.Remove(MoveDirections.Left);

            if (botX != mazeSize - 1 && (maze[botY, botX + 1] == wallPrint || maze[botY, botX + 1] == solverPrint))
                validDir.Remove(MoveDirections.Right);

            if (botY != 0 && (maze[botY - 1, botX] == wallPrint || maze[botY - 1, botX] == solverPrint))
                validDir.Remove(MoveDirections.Up);

            if (botY != mazeSize - 1 && (maze[botY + 1, botX] == wallPrint || maze[botY + 1, botX] == solverPrint))
                validDir.Remove(MoveDirections.Down);


            if (validDir.Contains(MoveDirections.Right)) 
                return MoveDirections.Right;
            
            else if (validDir.Contains(MoveDirections.Down)) 
                return MoveDirections.Down;
            
            else if(validDir.Contains(MoveDirections.Up)) 
                return MoveDirections.Up;

            else if(validDir.Contains(MoveDirections.Left))
                return MoveDirections.Left;

            return null;
        }
    }
}
