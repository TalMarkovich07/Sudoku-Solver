using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{

    internal class Program
    {
        
        static void Main(string[] args)
        {
            SudokuBoard b = new SudokuBoard("000000010400000000020000000000050407008000300001090000300400200050100000000806000");
            b.PrintBoard();
        }
    }
}
