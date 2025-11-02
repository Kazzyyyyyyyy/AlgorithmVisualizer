using System.CodeDom;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
    
    public enum Directions { Left, Right, Up, Down }

    public class DataMaze() {
        public static int mazeSize = 200;
        public static char[,] maze = new char[mazeSize, mazeSize];
        public static int startY = 1, startX = 0,
                          finishY = mazeSize - 2,
                          finishX = mazeSize - 1;
        public static bool mazeCurrentlySolved = false;

        public const char freeCellPrint = ' ', 
                          wallPrint = 'X',
                          solverPrint = 'G';
    }

    public class DataCpp {
        public static HashSet<(int y, int x)> cppFinalPathHashSet = new();
        public static List<(int y, int x)> cppFinalPathList = new();
        public static bool cppMazeDataLoaded = false;
        public static string cppError = string.Empty;

        public static float cppTime = 0;
        
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct CppDataStruct {
            //C#
            public int mmfMazeSize;

            //C++ 
            public int cppTime;
            public int cppFinalPathLength;
        };
    }

    public class DataMMF {
        public static readonly string name = "MazeAlgorithmData"; //C# AND C++ HAVE TO BE IDENTICAL!!!!!
    }

    public class DataGenerator {
        public static bool endReached = false,
                           imperfectMaze = true;
        public static int botY = 1, botX = 1;//DONT let the gen start on the border (0) bc he only checks if he CAN go there 
                                             //but if hes already there he can move freely and destroy the border
        public static List<(int y, int x)> moveHistory = new();
        public static List<Directions> validDir = new();

        public static void Reset() {
            endReached = false;
            botY = 1; botX = 1;
            moveHistory.Clear();
        }
    }

    public class DataAStar {
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
        public class Node {
            public int Y, X;
            public Node Parent;

            public Node(int y, int x, Node parent = null!) {
                Y = y;
                X = x;
                Parent = parent;
            }
        }

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
        public static Directions lookDir = Directions.Right,
                                 handSide; 
        public static void Reset() {
            botPos = (1, 0);
            lookDir = Directions.Right;
        }
    }
    
    public class DataRandomMove {
        public static int botY = 1, botX = 0;
        public static List<Directions> validDir = new();

        public static void Reset() {
            botY = 1; botX = 0;
        }
    }

    public class DataVisualizer {
        public static WriteableBitmap visualBitmap = null!;
        public static byte[] pixelArray = null!;
        public static int bytesPerPixel = 4,
                          cellSize = 5,
                          erasedMarkedCells = 0,
                          finalPathLength = 0,
                          animationRuns = 0,
                          animationSpeed = 50,
                          finalPathCellsVisualized = 0; 

        public static Color LastWantedColor;

        public static bool playAlgorithmAnimation = false,
                           highlightCurrentPos = false,
                           posHighlighted = false,
                           runCppAlgorithm = false,
                           currVisualizingFinalPath = false; 

        public static List<(int y, int x)> visualizerUpdateCords = new();

        public static (int y, int x, Color c) highlightedPos;

        public static readonly Color backgroundCol = (Color)ColorConverter.ConvertFromString("#3C3C3C"),
                                     wallCol = Colors.White, 
                                     solverCol = Colors.Green,
                                     csSolverFinalPathCol = Colors.Red,
                                     cppSolverFinalPathCol = Colors.Blue,
                                     bothSolversFinalPathCol = Colors.MediumPurple,
                                     mazeGenBacktrackCol = Colors.Gray, 
                                     solverPosHighlightCol = Colors.Red;
    }

    public class DataGeneral {

        public static Utils _utils = new();
        public static Visualizer _visl = new();
        public static MMFManager _mem = new();
        public static MazeGenerator _mazeGenerator = new();
        public static MazeSolver_RightOrLeftHand _dirHand = new();
        public static MazeSolver_BFS _bfs = new();
        public static MazeSolver_Random _random = new();
        public static MazeSolver_DeadEndFilling _deadEndFill = new();
        public static MazeSolver_A_Star _a_Star = new();
        
        //utils 
        public static Random rndm = new();
        public static Stopwatch timer = new();
    }
}
