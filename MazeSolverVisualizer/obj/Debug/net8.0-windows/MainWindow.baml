   M S B A M L   `   `   `  ����O  KMazeSolverVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:MazeSolverVisualizerMazeSolverVisualizer  5   	   %    MazeSolverVisualizer.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:MazeSolverVisualizer     5   	     � Title$  Algo visualizer��5   	    � WindowState$ 	Maximized=�6!   $
��1080��69   $
��1920��6G   ���#3C3C3C.��5      �  � ColumnDefinitions 5   
   �� 5      $��5* �6   �� 5      $��* �6   .+�5   
   � -   5         �Name$ GUI_controlls��5      $��1��6&   .+�5      �� -   5         ��Name$ GUI_generateMaze��5      $��Generate Maze��6N   $	��Top=�5      $
��Left=�6-   $��25��6H   $��5q�6T   �� 5      -   5        $ GUI_resetMaze��5      $��
Clean Maze��6H   $	��Top=�5      $��Right=�6-   $��	0,5,159,0q�6I   $��25��6\   �� 5      -   5         ��Name$ GUI_easyMazeToggle��5      $��	Easy Maze��63   $	��Top=�5      $
��Left=�6/   $��
175,12,0,0q�6J    d� 	IsChecked$
 TrueY�5      �� 5!      -   5         ��Name$ GUI_cellSize��5!      $	��Top=�5"      $
��Left=�6.   $��50��6I   $��5,35,0,0q�6T   �� 5$      -   5         ��Name$ GUI_cellSizeText��5$      $��:Visualizer Size��62   $	��Top=�5%      $
��Left=�60   $��	60,35,0,0q�6K   �� 5'      -   5        $ GUI_mazeSize��5'      $	��Top=�5(      $
��Left=�6.   $��50��6I   $��5,56,0,0q�6T   �� 5*      -   5        $ GUI_mazeSizeText��5*      $��
:Maze Size��62   $	��Top=�5+      $
��Left=�60   $��	60,56,0,0q�6K   �� 5.      -	   5        $ 	GUI_solve��5.      $��
Solve Maze��6@   $	��Top=�5/      $
��Left=�6-   $��5,97,0,0q�6H   $��25��6Z   �� 51      -
   5        $��GUI_solverSelect��51      $	��Top=�52      $
��Left=�6/   $��25��6J   $��	80,96,0,0q�6V   $	��105��6i   .@�53      �� $	��BFS��6   �� 54      $��RightWinder��6   �� 55      $��Random��6   56      �� 5:      -   5        $ GUI_animationToggle��5:      $��Algorithm Animation��64   $	��Top=�5;      $
��Left=�6/   $��5, 147q�6J   $
 TrueY�5<      �� 5>      -   5        $ GUI_animationSleep��5>      $	��Top=�5?      $
��Left=�6.   $��50��6I   $��5, 167q�6T   �� 5A      -   5        $ GUI_animationSleepText��5A      $��:Animation Speed��68   $	��Top=�5B      $
��Left=�60   $��60, 167q�6K   5C      �� 5G   
   -   5        	 ��Name$	 GUI_scrollbar��5G      $
��Auto=�5H      $
��Auto=�6<   .��5I      �� -   5        
 ��Name$
 GUI_visualizer��5I      $
��2100��5J      $
��2100��6"   $��Center=�60   $��Center=�6M   .+�5K      �� -   5         ��Name$ GUI_visualizerImage��5K      5L      5M      5N      5O      