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
        public Validation valid;
        public int[,] mat;
        public int[,] fails;

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
            fails = new int[MatSize, MatSize];
            if (input.Length != Math.Pow(MatSize, 2))
                throw new WrongLengthException();
            for (int row = 0; row < MatSize; row++)
                for (int col = 0; col < MatSize; col++)
                    mat[row, col] = CharToNum(input[MatSize*row + col]);
            this.valid = new Validation(mat);
        }
        public void PrintBoard()
        {
            for(int row= 0; row < MatSize; row++)
            {
                Console.WriteLine(new string('-', (int)(MatSize * 4.5)));
                if (row > 0 && row%BoxSize==0)
                    Console.WriteLine(new string('-', (int)(MatSize * 4.5)));
                for(int col = 0; col < MatSize; col++)
                {
                    Console.Write("|");
                    if (col > 0 && col % BoxSize == 0)
                        Console.Write(" |");
                    if (mat[row, col] == 0)
                        Console.Write(" ".PadRight(3));
                    else
                        Console.Write(mat[row, col].ToString().PadRight(3));
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n");
        }
        public void UpdateBoard(int row, int col, int num)
        {
            mat[row, col] = num;   
        }
        public bool BoardSolved()
        {
            for (int row = 0; row < MatSize; row++)
                for (int col = 0; col < MatSize; col++)
                    if (mat[row, col] == 0)
                        return false;
            return true;
        }
        
    }
}
