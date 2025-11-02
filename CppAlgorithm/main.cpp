#include <iostream> 
#include <windows.h> 
#include "bfs.h"
#include <chrono> 

using namespace std; 
using namespace chrono; 

class MMF {

  private: 
    const char *MMF_NAME = "MazeAlgorithmData"; //C# AND C++ HAVE TO BE IDENTICAL!!!!!

    HANDLE mmf = OpenFileMappingA(
        FILE_MAP_ALL_ACCESS,
        FALSE,
        MMF_NAME
    );

    void *mapped = MapViewOfFile(
        mmf, 
        FILE_MAP_ALL_ACCESS, 
        0,
        0,
        0
    );

    struct MMFDataStruct {
      //C#
      const int mazeSize; 

      //C++
      int cppTime; 
      int cppFinalPathLength; 
    }__attribute__((packed));
    
  public:
    char *maze;
    MMFDataStruct *data = (MMFDataStruct*)mapped;
    
    void read_mmf() {
      size_t offset = sizeof(MMFDataStruct); //start reading after struct

      maze = new char[data->mazeSize * data->mazeSize];
      for (int y = 0; y < data->mazeSize; y++) {
        for (int x = 0; x < data->mazeSize; x++) {
          maze[y * data->mazeSize + x] = *((char*)mapped + offset);
          offset += sizeof(char);
        }
      }
    }

    void send_mmf(int timeInMcs, int *finalPath) {
      data->cppTime = timeInMcs;

      int* pathDestination = (int*)((char*)mapped + sizeof(MMFDataStruct)); //points to the memory right after MMFDataStruct
      int pathLength = 0; 
      constexpr int PATH_END = INT_MIN;

      for(int i = 0; i < data->mazeSize * data->mazeSize; i++) {
        if(finalPath[i] == PATH_END) //'stop sign' because after the integers, the array is not initialized and contains garbage value
          break;

        pathDestination[i] = finalPath[i];
        pathLength++;
      }

      data->cppFinalPathLength = pathLength; 
    }
}; 

MMF mem; 
BFS bfs; 

int main() {

  mem.read_mmf(); 

  auto start = high_resolution_clock::now();

  int *finalPath = bfs.solve(mem.maze, mem.data->mazeSize, (mem.data->mazeSize - 2) * mem.data->mazeSize + mem.data->mazeSize - 1); 
  
  auto end = high_resolution_clock::now(); 
  auto time = duration_cast<microseconds>(end - start);

  mem.send_mmf(time.count(), finalPath);

  return 0;
}
