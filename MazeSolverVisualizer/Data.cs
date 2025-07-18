using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace MazeSolverVisualizer {
    public class Data {

        //classes 
        public static Utils _utils = new();
        public static MazeGenerator _mazeGenerator = new();
        public static MazeSolver_RightWind _rightWind = new();
        public static MazeSolver_BFS _bfs = new();
        public static MazeSolver_Random _random = new();

        //utils 
        public static Random rndm = new();
        
        //both
        public static int botY = 1, botX = 0;
        public enum MoveDirections { Left = 0, Right, Up, Down }
        public static List<MoveDirections> validDir = new();
        public static List<(int y, int x)> moveHistory = new(), 
                                           visUpdateCords = new();
        
        //generator specific
        public static char genPrint = ' ';
        public static bool endReached = false, 
                           easyMaze = true;

        //solver specific
        public static char solvPrint = 'G';
        
        //maze
        public static int mazeSize = 200;
        public static char[,] maze = new char[mazeSize, mazeSize];
        public static List<(int y, int x)> updateForCleanMazeVisual = new();

        //GUI
        public static Color backgroundCol = (Color)ColorConverter.ConvertFromString("#3C3C3C");
        public static bool playSolveAnimation = true;

        public static WriteableBitmap visualBitmap = null!;
        public static byte[] pixelArray = null!;
        public static int animationRoundNum = 0,
                          animationTaskDelayIn = 100;
        public static int bytesPerPixel = 4,
                          cellSize = 5;
    }
}
