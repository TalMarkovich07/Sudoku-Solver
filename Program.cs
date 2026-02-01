using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{

    internal class Program
    {
        public static string Input = "000000010400000000020000000000050407008000300001090000300400200050100000000806000";
        static void Main(string[] args)
        {
            RunTests tests = new RunTests();
            tests.Run();
            /*Solver solver = new Solver(Input);
            solver.board.PrintBoard();
            solver.Solve();
            solver.board.PrintBoard();
            Console.WriteLine($"{solver.iterations} iterations");*/
        }
    }
}
