using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{
    public class SudokuExceptions : Exception
    {
        public SudokuExceptions() { }

        public SudokuExceptions(string message) : base(message) { }

    }
    public class UnsolvableBoardException : SudokuExceptions
    {
        public UnsolvableBoardException() : base("Given board is unsolvable") { }
    }
    public class WrongLengthException : SudokuExceptions 
    {
        public WrongLengthException() : base($"Board dimensions must be {SudokuBoard.MatSize}x{SudokuBoard.MatSize}!") { }   
    }
}
