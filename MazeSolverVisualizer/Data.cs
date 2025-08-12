using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace MazeSolverVisualizer {
    
    public enum SolverAlgorithms { 
        AStar, 
        BFS, 
        DeadEndFilling, 
        RightHand, 
        LeftHand, 
        Random 
    }

    public class Node {
        public int Y, X, G, H;
        public int F => G + H;
        public Node Parent;

        public Node(int y, int x, int g = 0, int h = 0, Node parent = null!) {
            Y = y;
            X = x;
            G = g;
            H = h;
            Parent = parent;
        }
    }

    public class Data {
        public static bool endReached = false,
                          easyMaze = true;
        public static int botY = 1, botX = 0;

        //classes 
        public static Utils _utils = new();
        public static MazeGenerator _mazeGenerator = new();
        public static MazeSolver_RightOrLeftWind _dirWind = new();
        public static MazeSolver_BFS _bfs = new();
        public static MazeSolver_Random _random = new();
        public static MazeSolver_DeadEndFilling _deadEndFill = new();
        public static MazeSolver_A_Star _a_Star = new();

        //utils 
        public static Random rndm = new();
        public static bool playSolveAnimation = true;


        //both
        public static int mazeSize = 200;
        public static int startY = 1, startX = 0,
                          finishY = mazeSize - 2, finishX = mazeSize -1;

        //A* 
        public static int aStarGreed = 3;
        public static HashSet<(int, int)> closedSet = new();
        public static PriorityQueue<Node, int> openList = new();
        public static Dictionary<(int, int), Node> openSet = new();

        //BFS 
        public static Queue<Node> queue = new();

        //def
        public static int cellsBlockedThisRound = -1;


        public static Stopwatch timer = new();

        public static WriteableBitmap visualBitmap = null!;
        public static byte[] pixelArray = null!;
        public static Color backgroundCol = (Color)ColorConverter.ConvertFromString("#3C3C3C");
        public static int animationRoundNum = 0,
                          animationTaskDelayIn = 100;
        public static int bytesPerPixel = 4,
                          cellSize = 5;


        public enum MoveDirections { Left = 0, Right, Up, Down }
        public static List<MoveDirections> validDir = new();
        public static List<(int y, int x)> moveHistory = new(), 
                                           visUpdateCords = new();

        //solver specific
        public static Node current = null!;

        //maze
        public static char[,] maze = new char[mazeSize, mazeSize];
        public static List<(int y, int x)> updateForCleanMazeVisual = new();

        //prints
        public const char freeCellPrint = ' ';
        public const char wallPrint = 'X';
        public const char solverPrint = 'G';
    }
}
