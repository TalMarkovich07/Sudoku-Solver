using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{

    internal class Program
    {
        public static string Input = "000005080000601043000000000010500000000106000300000005530000061000000004000000000";
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
