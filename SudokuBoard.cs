using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{
    internal class SudokuBoard
    {
        public static int MatSize = 9;
        public static int BoxSize = (int)Math.Sqrt(MatSize);
        public int[,] mat;

        private int CharToNum(char c)
        {
            if (char.IsDigit(c))
                return c - '0';

            if (char.IsLetter(c))
            {
                int num = char.ToUpper(c) - 'A' + 10;
                if (num <= MatSize)
                    return num;
            }
            throw new SudokuExceptions($"Char {c} has no meaning for {MatSize}x{MatSize} Sudoku.");
        }
        public SudokuBoard(string input) 
        {
            mat = new int[MatSize, MatSize];
            if (input.Length != Math.Pow(MatSize, 2))
                throw new WrongLengthException();
            for (int row = 0; row < MatSize; row++)
                for (int col = 0; col < MatSize; col++)
                    mat[row, col] = CharToNum(input[MatSize*row + col]);
        }
        public void PrintBoard()
        {
            for(int row= 0; row < MatSize; row++)
            {
                if (row > 0 && row%BoxSize==0)
                    Console.WriteLine(new string('-', MatSize*4));
                for(int col = 0; col < MatSize; col++)
                {
                    if (col > 0 && col % BoxSize == 0)
                        Console.Write("| ");
                    Console.Write(mat[row, col].ToString().PadRight(3));
                }
                Console.WriteLine();
            }
        }
        public void UpdateBoard(int row, int col, int num)
        {
            mat[row, col] = num;   
        }
    }
}
