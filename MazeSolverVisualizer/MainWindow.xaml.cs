using System.Windows;
using System.Windows.Controls;

using static MazeSolverVisualizer.Data;


namespace MazeSolverVisualizer {

    public partial class MainWindow : Window {

        //main        
        public static MainWindow _mainWindow { get; private set; } = null!;

        public MainWindow() {
            InitializeComponent();
            _mainWindow = this;
            Init();

            _utils.CreateOrUpdateVisualizer();
        }

        void Init() {
            GUI_visualizer.Width = mazeSize * cellSize;
            GUI_visualizer.Height = mazeSize * cellSize;

            GUI_animationSleep.Text = animationTaskDelayIn.ToString();
            GUI_cellSize.Text = cellSize.ToString();

            GUI_mazeSize.Text = mazeSize.ToString();
            
            GUI_solverSelect.SelectedIndex = 0;
        }


        //Controll events
        private void GUI_generateMaze_Click(object sender, RoutedEventArgs e) {
            _utils.BlockGUIEventsWhileRunning();

            MazeGenerator.CallGenerator();
        }

        private void GUI_resetMaze_Click(object sender, RoutedEventArgs e) 
            => _utils.CleanVisualizer();
        
        private void GUI_solve_Click(object sender, RoutedEventArgs e) {
            _utils.BlockGUIEventsWhileRunning();

            //GUI parse
            if (int.TryParse(_mainWindow.GUI_animationSleep.Text, out int parse))
                animationTaskDelayIn = parse;
            else
                _mainWindow.GUI_animationSleep.Text = animationTaskDelayIn.ToString();
            

            if(updateForCleanMazeVisual.Count > 0)
                _utils.CleanVisualizer();


            switch (GUI_solverSelect.SelectedIndex) {
                case 0:
                    MazeSolver_BFS.CallSolver_BFS(); break;

                case 1: 
                    MazeSolver_RightWind.CallSolver_RightWind(); break;

                case 2:
                    MazeSolver_Random.CallSolver_Random(); break;

                default: return;
            }
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

        private void GUI_animationSleep_TextChanged(object sender, TextChangedEventArgs e) {
            if (int.TryParse(GUI_animationSleep.Text, out int parsed))
                animationTaskDelayIn = parsed;

            else if (GUI_animationSleep.Text.Length == 0)
                animationTaskDelayIn = 0;
        }
    }
}
