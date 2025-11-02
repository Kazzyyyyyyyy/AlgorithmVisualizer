using System.Windows;
using System.Windows.Controls;
using static MazeSolverVisualizer.DataGeneral;
using static MazeSolverVisualizer.Utils;
using static MazeSolverVisualizer.DataCpp;
using static MazeSolverVisualizer.DataAStar;
using static MazeSolverVisualizer.DataVisualizer;
using static MazeSolverVisualizer.DataMaze;
using System.Windows.Media;
using System.IO;


namespace MazeSolverVisualizer {

    public partial class MainWindow : Window {

        //main
        public static MainWindow _mainWindow { get; private set; } = null!;

        public MainWindow() {
            InitializeComponent();
            _mainWindow = this;
            GUIInit();


            ///programm start
            GUI_solverSelector.SelectedItem = SolverAlgorithms.AStar;
            GUI_button_Click(GUI_generateMaze, null!);
            playAlgorithmAnimation = true;
        }

        void GUIInit() {
            
            //visualizer
            GUI_visualizer.Width = mazeSize * DataVisualizer.cellSize;
            GUI_visualizer.Height = mazeSize * DataVisualizer.cellSize;

            //controls
            GUI_animationSpeed.Text = DataVisualizer.animationSpeed.ToString();
            GUI_visualizerCellSize.Text = DataVisualizer.cellSize.ToString();

            GUI_mazeSize.Text = mazeSize.ToString();

            GUI_imperfectMazeToggle.IsChecked = DataGenerator.imperfectMaze ? true : false;

            GUI_AStarGreed.Text = DataAStar.aStarGreed.ToString();

            GUI_csColorDisplay.Background = new SolidColorBrush(csSolverFinalPathCol);
            GUI_cppColorDisplay.Background = new SolidColorBrush(cppSolverFinalPathCol);
            GUI_bothColorDisplay.Background = new SolidColorBrush(bothSolversFinalPathCol);
        }


        //Control events
        private void GUI_resetMaze_Click(object sender, RoutedEventArgs e) {
            _utils.CleanupMazeArray();
            _visl.CreateOrUpdateVisualizer();
        }

        private async void GUI_button_Click(object sender, RoutedEventArgs e) {

            //animation speed parse
            if (int.TryParse(_mainWindow.GUI_animationSpeed.Text, out int parsedSpeed)) 
                DataVisualizer.animationSpeed = parsedSpeed;
            else
                _mainWindow.GUI_animationSpeed.Text = DataVisualizer.animationSpeed.ToString();


            _utils.DisAndEnableControls();

            if ((Button)sender == GUI_generateMaze)
                await GeneratorManager();

            else if ((Button)sender == GUI_solveMaze)
                await SolverManager();

            OutPutDataAfterRun();
            await ResetGlobalVars();
            
            _utils.DisAndEnableControls();
        }

        async Task GeneratorManager() {

            cppFinalPathHashSet.Clear();
            cppFinalPathList.Clear();

            //maze size parse
            if (int.TryParse(_mainWindow.GUI_mazeSize.Text, out int parsedSize) && parsedSize != mazeSize) {
                mazeSize = parsedSize;
                maze = new char[mazeSize, mazeSize];

                finishY = mazeSize - 2;
                finishX = mazeSize - 1;
            }
            else
                _mainWindow.GUI_mazeSize.Text = mazeSize.ToString();


            _utils.FillMazeArrayWithWalls();

            if (playAlgorithmAnimation)
                _visl.CreateOrUpdateVisualizer();

            await MazeGenerator.CallGenerator();

            mazeCurrentlySolved = false; 
        }

        async Task SolverManager() {

            if (mazeCurrentlySolved) {
                _utils.CleanupMazeArray();

                if (playAlgorithmAnimation) {
                    bool visualizeCppPath = false;
                    _visl.CreateOrUpdateVisualizer(visualizeCppPath);
                }
            }

            if (runCppAlgorithm && cppFinalPathHashSet.Count == 0)
                MMFManager.CallMemoryTransfer(); //run c++ and get data

            var solver = GUI_solverSelector.SelectedItem switch {
                SolverAlgorithms.AStar => MazeSolver_A_Star.CallSolver_A_Star(),
                SolverAlgorithms.BFS => MazeSolver_BFS.CallSolver_BFS(),
                SolverAlgorithms.DeadEndFilling => MazeSolver_DeadEndFilling.CallSolver_DeadEndFilling(),
                SolverAlgorithms.RightHand => MazeSolver_RightOrLeftHand.CallSolver_RightOrLeftHand(Directions.Right),
                SolverAlgorithms.LeftHand => MazeSolver_RightOrLeftHand.CallSolver_RightOrLeftHand(Directions.Left),
                SolverAlgorithms.Random => MazeSolver_Random.CallSolver_Random(),
                _ => Task.CompletedTask
            };

            await solver;
            mazeCurrentlySolved = true;
        }

        private void GUI_solverSelector_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            GUI_outPut.Text = "";

            //BFS is the only algorithm I made in C++
            if ((SolverAlgorithms)GUI_solverSelector.SelectedItem == SolverAlgorithms.BFS) {
                GUI_runCppAlgorithm.IsEnabled = true;
                runCppAlgorithm = (bool)GUI_runCppAlgorithm.IsChecked!;
            }
            else {
                runCppAlgorithm = false;
                GUI_runCppAlgorithm.IsEnabled = false;
                cppFinalPathHashSet.Clear();
                cppFinalPathList.Clear();
            }

            if ((SolverAlgorithms)GUI_solverSelector.SelectedItem == SolverAlgorithms.DeadEndFilling && GUI_imperfectMazeToggle.IsChecked == true) {
                GUI_outPut.Text = @"DeadEndFilling doesnt properly work if ImperfectMaze is on. The generator makes a 'imperfect maze' (more than 1 linear way to the finish) when checked and DeadEndFilling is made for 'perfect mazes'.";
                return;
            }
            else if ((SolverAlgorithms)GUI_solverSelector.SelectedItem == SolverAlgorithms.AStar) {
                GUI_AStarGreed.Visibility = Visibility.Visible;
                GUI_AStarGreedText.Visibility = Visibility.Visible;
                return;
            }

            GUI_AStarGreed.Visibility = Visibility.Collapsed;
            GUI_AStarGreedText.Visibility = Visibility.Collapsed;
        }

        private void GUI_animationToggle_Clicked(object sender, RoutedEventArgs e) {
            if(GUI_animationToggle.IsChecked == true) {
                GUI_animationSpeed.Visibility = Visibility.Visible;
                GUI_animationSpeedText.Visibility = Visibility.Visible;
                playAlgorithmAnimation = true; 
                return; 
            }

            GUI_animationSpeedText.Visibility = Visibility.Collapsed;
            GUI_animationSpeed.Visibility = Visibility.Collapsed;
            playAlgorithmAnimation = false;
        }

        private void GUI_imperfectMazeToggle_Click(object sender, RoutedEventArgs e)
            => DataGenerator.imperfectMaze = GUI_imperfectMazeToggle.IsChecked == true ? true : false;

        private void GUI_highlightCurrentPos_Click(object sender, RoutedEventArgs e) 
            => highlightCurrentPos = GUI_highlightCurrentPos.IsChecked == true ? true : false;

        private void GUI_runCppAlgorithm_Click(object sender, RoutedEventArgs e) {
            runCppAlgorithm = GUI_runCppAlgorithm.IsChecked == true ? true : false;
            if(!runCppAlgorithm) {
                cppFinalPathList.Clear();
                cppFinalPathHashSet.Clear();
            }
        }
    }
}
