using System.CodeDom;
using System.Diagnostics;
using System.Security;
using System.Windows;
using System.Windows.Controls;
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

    public class Node { //for A* and BFS 
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


    public class DataGenerator {
        public static bool endReached = false,
                           imperfectMaze = true;
        public static int botY = 1, botX = 1;//DONT let the gen start on the border (0) bc he only checks if he CAN go there 
                                             //but if hes already there he can move freely and destroy the border
        public static List<(int y, int x)> moveHistory = new();

        public static void Reset() {
            endReached = false;
            botY = 1; botX = 1;
            moveHistory.Clear();
        }
    }

    public class DataAStar {
        public static int aStarGreed = 3;
        public static HashSet<(int, int)> closedSet = new();

        public static PriorityQueue<Node, int> openList = new();

        public static Dictionary<(int, int), Node> openSet = new();
        public static Node current = null!;

        public static void Reset() {
            current = null!;
            closedSet.Clear();
            openList.Clear();
            openSet.Clear();
        }
    }

    public class DataBFS {
        public static Queue<Node> queue = new();
        public static Node current = null!;
        public static HashSet<(int, int)> closedSet = new();

        public static void Reset() {
            queue.Clear();
            current = null!;
            closedSet.Clear();
        }
    }

    public class DataDeadEndFill {
        public static int cellsBlockedThisRound = -1;
        public static (int Y, int X) current;
        public static Queue<(int, int)> deadEndQueue = new();

        public static void Reset() {
            cellsBlockedThisRound = -1;
            deadEndQueue.Clear();
        }
    }

    public class DataRightOrLeftHand {
        public static (int Y, int X) botPos = (1, 0);
        public static DataGlobal.Directions lookDir = DataGlobal.Directions.Right;
        public static DataGlobal.Directions handSide; 
        public static void Reset() {
            botPos = (1, 0);
            lookDir = DataGlobal.Directions.Right;
        }
    }
    
    public class DataRandomMove {
        public static int botY = 1, botX = 0;

        public static void Reset() {
            botY = 1; botX = 0;
        }
    }

    public class DataVisualizer {
        public static WriteableBitmap visualBitmap = null!;
        public static byte[] pixelArray = null!;
        public static int animationSpeed = 50;
        public static int bytesPerPixel = 4,
                          cellSize = 5;
        public static int animationRuns = 0;
        public static Color LastWantedColor;
    }

    public class DataGlobal {

        //classes 
        public static Utils _utils = new();
        public static Visualizer _visl = new();
        public static MazeGenerator _mazeGenerator = new();
        public static MazeSolver_RightOrLeftHand _dirHand = new();
        public static MazeSolver_BFS _bfs = new();
        public static MazeSolver_Random _random = new();
        public static MazeSolver_DeadEndFilling _deadEndFill = new();
        public static MazeSolver_A_Star _a_Star = new();

        //public algorithm
        public enum Directions { Left, Right, Up, Down }
        public static List<Directions> validDir = new();

        //utils 
        public static Random rndm = new();
        public static Stopwatch timer = new();
        public static bool mazeCurrentlySolved = false;
        public static int erasedMarkedCells = 0, finalPathLength = 0;

        //visualizer
        public static bool playAlgorithmAnimation = false,
                           highlightCurrentPos = false,
                           posHighlighted = false;

        public static (int y, int x, Color c) highlightedPos;
        public static List<(int y, int x)> visualizerUpdateCords = new();
        public static Color backgroundCol = (Color)ColorConverter.ConvertFromString("#3C3C3C");

        //maze
        public static int mazeSize = 200;
        public static char[,] maze = new char[mazeSize, mazeSize];
        public static int startY = 1, startX = 0,
                          finishY = mazeSize - 2, 
                          finishX = mazeSize -1;

        //prints
        public const char freeCellPrint = ' ';
        public const char wallPrint = 'X';
        public const char solverPrint = 'G';
    }
}
