class BFS {
  public: 
    inline int* solve(char *maze, const size_t &mazeSize, const size_t &finish) {
      int *path = new int[mazeSize * mazeSize];
      int *queue = new int[mazeSize * mazeSize];

      int *finalPath = new int[mazeSize * mazeSize]; //originally I reused queue for finalPath, 
                                                     //but making and using a new array is ~200mcs faster
      size_t queHead = 0, queTail = 0; 

      maze[mazeSize] = '.';
      path[mazeSize] = -1; 
      queue[queTail++] = mazeSize; 

      int curr;
      while(maze[finish] != '.') {
        curr = queue[queHead++]; 
        const int currY = curr / mazeSize;
        const int currX = curr % mazeSize;


        const int down = (currY + 1) * mazeSize + currX;
        if(maze[down] == ' ') {
          path[down] = curr;
          queue[queTail++] = down;
          maze[down] = '.';
        }

        const int right = currY * mazeSize + currX + 1;
        if(maze[right] == ' ') {
          path[right] = curr;
          queue[queTail++] = right;
          maze[right] = '.';
        }

        const int up = (currY - 1) * mazeSize + currX;
        if(maze[up] == ' ') {
          path[up] = curr;
          queue[queTail++] = up;
          maze[up]= '.';
        }
    
        const int left = currY * mazeSize + currX - 1;
        if(maze[left] == ' ') {
          path[left] = curr;
          queue[queTail++] = left;
          maze[left] = '.';
        } 
      }


      finalPath[0] = curr + 1; //BFS always stoppes 1 before the actuall finish, so we need to add it manually

      size_t round = 1;
      while(curr != -1) {
        finalPath[round] = curr;
        curr = path[curr];
        round++;
      }

      constexpr int PATH_END = INT_MIN;
      finalPath[round] = PATH_END;
      return finalPath;
    }
};
