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

            //start prepare
            moveHistory.Add((botY, botX));
            visUpdateCords.Add((botY, botX));
            maze[botY, botX] = solvPrint;


            while (RunLoop_Solver()) {
                MoveDirections? currDir = GetMoveDirection();

                if (currDir == null) {
                    _utils.Backtrack();
                    continue;
                }

                MoveBot(currDir);

                moveHistory.Add((botY, botX));
                maze[botY, botX] = solvPrint;

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
            if (botX != 0 && (maze[botY, botX - 1] == 'X' || maze[botY, botX - 1] == solvPrint))
                validDir.Remove(MoveDirections.Left);

            if (botX != mazeSize - 1 && (maze[botY, botX + 1] == 'X' || maze[botY, botX + 1] == solvPrint))
                validDir.Remove(MoveDirections.Right);

            if (botY != 0 && (maze[botY - 1, botX] == 'X' || maze[botY - 1, botX] == solvPrint))
                validDir.Remove(MoveDirections.Up);

            if (botY != mazeSize - 1 && (maze[botY + 1, botX] == 'X' || maze[botY + 1, botX] == solvPrint))
                validDir.Remove(MoveDirections.Down);


            //final decision or backtrack 
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
