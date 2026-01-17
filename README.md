# AlgorithmVisualizer

C#/WPF UI programm to watch algorithms do their thing in real time. 

## Features 
 - MazeGenerator that can make perfect aswell as imperfect mazes 
 - 6 solve algorithms to choose from
 - settings to change MazeSize, UISize, AnimationSpeed, Animation Toggle and more
 - good UI with color highlighting


## C++ Update

### New Features
 - MemoryMappedFile for C#/C++ interop
 - Live data interop for visualiziation and comparison in GUI (can be turned off) 
 - C++ ultra optimized BFS algorithm
 
### Personal notes
The plan was to port all algorithms from the C# project to C++, so you can compare every algorithm. But thats not really worth it. 
I learned everyhing there is to learn from this project. I made BFS, optimized it to the last bit and integrated it into the C# project. 
There is no real point in doing the same, but with 4 more algorithms just to "complete it", so I ended the project here. 

### Notes
**compilation:**
```
 g++ -O3 -march=native -mtune=native -flto -funroll-loops -ftree-vectorize -fstrict-aliasing -ffast-math -fomit-frame-pointer (main.cpp -o algo.exe)
```
 