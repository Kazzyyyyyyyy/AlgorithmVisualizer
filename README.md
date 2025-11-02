After I made my first Algorithm, I was really glued to it. 
It was incredibly fun, and I wanted to tackle some more, bigger, and cooler algorithms. 
So, I decided this would be my 4th big C# project.

---------------------------------------------------

My focus (again) was on writing clean and good code so that others could easily work on it and add algorithms or other stuff.

Originally, I made the Maze Generation algorithm and the Right-Hand Rule algorithm as two separate console app prototypes. 
Then I decided to scale up to a WPF application with animations, controls, and even more algorithms. 
In the end, theres one Gen algorithm and five solve algorithms (A*, BFS, DeadEndFill, Right/LeftHand and Random).


As always, I tried to do as much as possible myself. 
The generation, Random, and Right/LeftHand algorithms were all made 100% by myself (idea and coding) without any Googling. 
With the BFS, I only read an explanation of how it works and understood it very quickly, for A* I needed quite some help.


This is by far my best and coolest project I made to this day (August 13, 2025). 
Im very proud and think I managed to achieve my goals (mainly clean/better code). 
I believe the code is good enough for other people to work on, so please feel free to add something if you want! 
The project will also be upgraded soon; next, I want to add some sorting algorithms.


There's still one bug I couldnt really fix yet: If the animation runs (doesnt matter which one) and you focus something in the GUI (Text/ComboBox for example), 
the animation speeds up significantly while the GUI is focused. 
I think its because of WPF prioritizing some threads, but I couldnt really figure it out yet.
The biggest struggle of the project wasnt even really the algorithms, but the GUI stuff. 
I had a lot of performance problems with big mazes but was able to fix most of it. 
And it runs very well now. 

---------------------------------------------------

I learned a lot in this project, especially about clean and maintainable code, and of course about algorithms (which are very fun).

Like I said; if you want to add something to the program yourself, feel free to do that!

---------------------------------------------------

# C++ Update (yes, I know now how to use README´s)

## New Features
 - MemoryMappedFile for C#/C++ interop
 - Live data interop for visualiziation and comparing in GUI (can be turned off) 
 - C++ ultra optimized BFS algorithm
 
## What Iv'e learned 
 - Working with (temporary) MMF´s
 - Multi-language interop and data management
 - Working with pure memory (bytes, correct offset calculation, size of datatypes, differences in datatypes between C# and C++)
 - Optimization rules and methods for basic code and especially BFS/Algorithms

## Personal notes
The plan was to port all algorithms from the C# project to C++, so you can compare every algorithm. But thats not really worth it. 
I learned everyhing there is to learn from this project. I made BFS, optimized it to the last bit and integrated it into the C# project. 
There is no real point in doing the same, but with 4 more algorithms just to "complete it", so I ended the project here. 

## Notes
 - C++ compiler flags: g++ -O3 -march=native -mtune=native -flto -funroll-loops -ftree-vectorize -fstrict-aliasing -ffast-math -fomit-frame-pointer (main.cpp -o algo.exe)