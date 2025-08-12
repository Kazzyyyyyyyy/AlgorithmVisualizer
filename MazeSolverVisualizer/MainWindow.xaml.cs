using System.Windows;
using System.Windows.Controls;
using static MazeSolverVisualizer.Data;
using static MazeSolverVisualizer.Utils;


namespace MazeSolverVisualizer {

    public partial class MainWindow : Window {

        //main        
        public static MainWindow _mainWindow { get; private set; } = null!;

        public MainWindow() {
            InitializeComponent();
            _mainWindow = this;
            Init();

            _utils.BlockControlsWhileRunning();

            playSolveAnimation = false; 
            MazeGenerator.CallGenerator();
            playSolveAnimation = true;
        }

        void Init() {
            GUI_visualizer.Width = mazeSize * cellSize;
            GUI_visualizer.Height = mazeSize * cellSize;

            GUI_animationSleep.Text = animationTaskDelayIn.ToString();
            GUI_cellSize.Text = cellSize.ToString();

            GUI_mazeSize.Text = mazeSize.ToString();

            GUI_easyMazeToggle.IsChecked = easyMaze ? true : false;

            GUI_AStarGreed.Text = aStarGreed.ToString();

            GUI_solverSelect.SelectedItem = SolverAlgorithms.AStar;
        }


        //Control events
        private void GUI_generateMaze_Click(object sender, RoutedEventArgs e) {
            _utils.BlockControlsWhileRunning();

            MazeGenerator.CallGenerator();
        }

        private void GUI_resetMaze_Click(object sender, RoutedEventArgs e) 
            => _utils.CleanVisualizer();

        private async void GUI_solve_Click(object sender, RoutedEventArgs e) {
            _utils.BlockControlsWhileRunning();

            //GUI parse
            if (int.TryParse(_mainWindow.GUI_animationSleep.Text, out int parse))
                animationTaskDelayIn = parse;
            else
                _mainWindow.GUI_animationSleep.Text = animationTaskDelayIn.ToString();

            if (updateForCleanMazeVisual.Count > 0) {
                _utils.ReturnToCleanMaze();
                _utils.CleanVisualizer();
            }

            //algorithm start 
            if ((SolverAlgorithms)GUI_solverSelect.SelectedItem != SolverAlgorithms.DeadEndFilling) { //dont do when call DeadEndFilling 
                maze[botY, botX] = solverPrint;
                moveHistory.Add((botY, botX));
                if (playSolveAnimation)
                    visUpdateCords.Add((botY, botX));
            }

            timer.Start(); 

            switch (GUI_solverSelect.SelectedItem) {
                case SolverAlgorithms.AStar:
                    await MazeSolver_A_Star.CallSolver_A_Star(); break;

                case SolverAlgorithms.BFS:
                    await MazeSolver_BFS.CallSolver_BFS(); break;

                case SolverAlgorithms.DeadEndFilling:
                    await MazeSolver_DeadEndFilling.CallSolver_DeadEndFilling(); break;

                case SolverAlgorithms.RightHand:
                    await MazeSolver_RightOrLeftWind.CallSolver_RightOrLeftWind(MoveDirections.Right); break;

                case SolverAlgorithms.LeftHand:
                    await MazeSolver_RightOrLeftWind.CallSolver_RightOrLeftWind(MoveDirections.Left); break;

                case SolverAlgorithms.Random:
                    await MazeSolver_Random.CallSolver_Random(); break;

                default: return;
            }

            _utils.OutPutAfterSolve();
            
            ResetAllVars();
        }

        private void GUI_solverSelect_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if ((SolverAlgorithms)GUI_solverSelect.SelectedItem == SolverAlgorithms.DeadEndFilling && GUI_easyMazeToggle.IsChecked == true) {
                GUI_outPut.Text = @"DeadEndFilling doesnt properly work if EasyMaze is on. The generator makes a 'inperfect maze' (not only 1 linear way to the finish) when checked and DeadEndFilling is made for 'perfect mazes'.";
                return;
            }
            else if ((SolverAlgorithms)GUI_solverSelect.SelectedItem == SolverAlgorithms.AStar) {
                GUI_AStarGreed.Visibility = Visibility.Visible;
                GUI_AStarGreedText.Visibility = Visibility.Visible;
                return;
            }

            GUI_outPut.Text = "";
            GUI_AStarGreed.Visibility = Visibility.Collapsed;
            GUI_AStarGreedText.Visibility = Visibility.Collapsed;
        }

        private void GUI_animationToggle_Clicked(object sender, RoutedEventArgs e) {
            if(GUI_animationToggle.IsChecked == true) {
                GUI_animationSleep.Visibility = Visibility.Visible;
                GUI_animationSleepText.Visibility = Visibility.Visible;
                playSolveAnimation = true; 

                return; 
            }

            GUI_animationSleepText.Visibility = Visibility.Collapsed; 
            GUI_animationSleep.Visibility = Visibility.Collapsed;
            playSolveAnimation = false;
        }

        private void GUI_easyMazeToggle_Click(object sender, RoutedEventArgs e)
            => easyMaze = GUI_easyMazeToggle.IsChecked == true ? true : false;
    }
}
