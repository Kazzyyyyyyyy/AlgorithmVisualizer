using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using static MazeSolverVisualizer.DataGeneral;
using static MazeSolverVisualizer.DataMMF;
using static MazeSolverVisualizer.DataMaze;
using static MazeSolverVisualizer.DataCpp;
using System.Reflection;

namespace MazeSolverVisualizer {

    public class MMFManager {

        public static void CallMemoryTransfer()
           => _mem.MMFOperation();
        

        void MMFOperation() {
            try {
                int size = mazeSize * mazeSize + 22; //mazeSize * mazeSize for the maze, 12 for the struct and extra 10 bytes just to make sure its enough
                using (var mmf = MemoryMappedFile.CreateNew(name, size, MemoryMappedFileAccess.ReadWrite)) {
                    using (var ac = mmf.CreateViewAccessor()) {
                        CppDataStruct data = new CppDataStruct{ mmfMazeSize = mazeSize };

                        ac.Write(0, ref data);

                        int offset = Marshal.SizeOf(data); //start writing maze after struct
                        for (int y = 0; y < mazeSize; y++) {
                            for (int x = 0; x < mazeSize; x++) {
                                ac.Write(offset, maze[y, x]);
                                offset++; //offest += sizeof(char)
                            }
                        }

                        string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!; //gets the path where the programm is currently running
                        string cppPath = Path.Combine(exeDir, @"..\..\..\..\CppAlgorithm\algo.exe");      //does some calculation stuff to find the right path to folder CppAlgorithm (idk what exactly and also dont care tbh)
                        cppPath = Path.GetFullPath(cppPath);                                              //gets the path, so C# always finds the cpp part, no matter where C# runs

                        ProcessStartInfo startInfo = new ProcessStartInfo {
                            FileName = cppPath,
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            WindowStyle = ProcessWindowStyle.Hidden
                        };

                        Process proc = Process.Start(startInfo)!;
                        proc.WaitForExit();

                        
                        ac.Read(0, out data);

                        cppTime = data.cppTime;

                        offset = Marshal.SizeOf(data); //start reading after the struct
                        for (int i = 0; i < data.cppFinalPathLength; i++) {
                            int cords = ac.ReadInt32(offset);

                            cppFinalPathHashSet.Add((cords / mazeSize, cords % mazeSize)); //both because HashSet is better for instant visualization,
                            cppFinalPathList.Add((cords / mazeSize, cords % mazeSize));    //List for animation and at 1000x1000 mazeSize and ~5000 cells finalPath
                            offset += 4; //offest += sizeof(int)                           //both together are not even 40kb
                        }
                    }
                }
            }
            catch(FileNotFoundException ex) {
                cppError = ex.Message;
                return;
            }
            catch (Exception ex) {
                cppError = ex.Message;
                return;
            }

            cppError = string.Empty;
        }
    }
}



