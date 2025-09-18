using System.Windows;
using System.Windows.Media;

using static MazeSolverVisualizer.DataGlobal;
using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.DataRightOrLeftHand;
using static MazeSolverVisualizer.MainWindow;
using System.Security.Principal;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;

namespace MazeSolverVisualizer {
    public class MazeSolver_RightOrLeftHand {

        //main
        public static async Task CallSolver_RightOrLeftHand(Directions side) {
            timer.Start();
            handSide = side;
            await _dirHand.Loop();
            timer.Stop();
        }

        async Task Loop() {

            maze[startY, startX] = solverPrint;
            await _visl.UpdateVisualizerAtCoords((startY, startX), Colors.Green);

            while (RunLoop_Solver()) {
                SolveLogic();
            
                await _visl.UpdateVisualizerAtCoords(botPos, Colors.Green);
            }

            if (!playAlgorithmAnimation)
                _visl.CreateOrUpdateVisualizer();

            DataRightOrLeftHand.Reset();
        }

        //deep logic
        void SolveLogic() {

            lookDir = TurnToHandSide(); 

            for(int i = 0; i < 4; i++) {
                (int y, int x) next = GetNextCoords();
                
                if(IsWalkable(next)) {
                    botPos = next;
                    maze[botPos.Y, botPos.X] = solverPrint;
                    break; 
                }

                lookDir = TurnLeftOrRight();
            }
        }

        Directions TurnLeftOrRight() {

            if (handSide == Directions.Right) {
                switch (lookDir) {
                    case Directions.Right: return Directions.Up;
                    case Directions.Left: return Directions.Down;
                    case Directions.Up: return Directions.Left;
                    case Directions.Down: return Directions.Right;
                    default: return lookDir;
                }
            }

            //left
            switch (lookDir) {
                case Directions.Right: return Directions.Down;
                case Directions.Left: return Directions.Up;
                case Directions.Up: return Directions.Right;
                case Directions.Down: return Directions.Left;
                default: return lookDir;
            }
        }

        bool IsWalkable((int y, int x) next) {
            if (next.y < 0 || next.x >= mazeSize || maze[next.y, next.x] == wallPrint)
                return false;

            return true; 
        }

        (int, int) GetNextCoords() {
            switch(lookDir) {
                case Directions.Right: return (botPos.Y, botPos.X + 1);
                case Directions.Left: return (botPos.Y, botPos.X - 1);
                case Directions.Down: return (botPos.Y + 1, botPos.X);
                case Directions.Up: return (botPos.Y - 1, botPos.X); 
                default: return botPos;
            }
        }

        Directions TurnToHandSide() {
            
            if(handSide == Directions.Right) {
                switch (lookDir) {
                    case Directions.Right: return Directions.Down;
                    case Directions.Left: return Directions.Up;
                    case Directions.Up: return Directions.Right;
                    case Directions.Down: return Directions.Left;
                    default: return lookDir;
                }
            }

            //left
            switch (lookDir) {
                case Directions.Right: return Directions.Up;
                case Directions.Left: return Directions.Down;
                case Directions.Up: return Directions.Left;
                case Directions.Down: return Directions.Right;
                default: return lookDir;
            }
        }
    }
}
