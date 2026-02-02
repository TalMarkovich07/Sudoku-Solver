using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sudoku_Solver
{
    internal class RunTests
    {
        string[] sudokus = File.ReadAllLines("17_clue.txt");
        public void Run()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine($"Solving {sudokus.Length} sudokus:");
            for(int i = 0; i < sudokus.Length; i++)
            {
                Solver solver = new Solver(sudokus[i]);
                solver.Solve();
                if(solver.SolveTimeMs > 1000)
                    Console.WriteLine($"Board {i}: {sudokus[i]} took {solver.SolveTimeMs} to solve");
            }
            watch.Stop();
            Console.WriteLine($"whole run took {watch.ElapsedMilliseconds} milliseconds");
        }
    }
}
