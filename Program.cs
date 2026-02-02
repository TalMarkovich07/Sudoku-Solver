using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{

    internal class Program
    {
        public static string Input = "000060080020000000001000000070000102500030000000000400004201000300700600000000050";
        static void Main(string[] args)
        {
            try
            {
                RunTests tests = new RunTests();
                tests.Run();
                /*Solver solver = new Solver(Input);
                solver.board.PrintBoard();
                solver.Solve();
                solver.board.PrintBoard();
                Console.WriteLine($"{solver.SolveTimeMs} milliseconds");*/
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sudoku exception: {ex}");
            }
        }
    }
}
