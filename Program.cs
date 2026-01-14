using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{

    internal class Program
    {
        public static int MaxSize = 9;
        public static int[][] ToMat(string input)
        {
            if (input.Length != Math.Pow(MaxSize, 2))
                throw new SudokuException($"The board must be {Program.MaxSize}x{Program.MaxSize}.");
        }
        static void Main(string[] args)
        {
        }
    }
}
