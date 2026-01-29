using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{
    internal class Solver
    {
        public int iterations = 0;
        public SudokuBoard board;
        public Validation valid;
        public Solver(string input)
        {
            this.board = new SudokuBoard(input);
            this.valid = new Validation(board.mat);
        }
        private void UpdateSolver(int row, int col, int num, int numToClear = 0)
        {
            if (num == 0)
            {
                valid.ClearValid(row, col, numToClear);
                board.UpdateBoard(row, col, 0);
            }
            else
            {
                board.UpdateBoard(row, col, num);
                valid.UpdateValid(row, col, num);
            }
        }
        public (int, int) MinRemainValues()
        {
            int minOption = SudokuBoard.MatSize;
            int minRow = -1;
            int minCol = -1;
            for (int row = 0; row < SudokuBoard.MatSize; row++)
                for (int col = 0; col < SudokuBoard.MatSize; col++)
                {
                    if (board.mat[row, col] == 0)
                    {
                        int options = valid.CountOptions(row, col);
                        if (options == minOption && board.fails[row, col] > board.fails[minRow, minCol])
                        // if there are two cells with the same amount of options - choose the one that failed more
                        {
                            minRow = row;
                            minCol = col;
                        }
                        if (options < minOption)
                        {
                            minOption = options;
                            minRow = row;
                            minCol = col;
                        }

                    }

                }
            return (minRow, minCol);
        }
        public void Solve()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            if (RecursionSolve(0, 0))
            {
                watch.Stop();
                Console.WriteLine($"\nSolved in: {watch.Elapsed.TotalMilliseconds}ms");
            }
            else
                throw new SudokuExceptions("Given board is un-solvable");
        }
        private bool RecursionSolve(int row, int col)
        {
            iterations++;
            if (row == -1)
                return board.BoardSolved();
            int x, y;
            if (board.mat[row, col] != 0)
            {
                (x, y) = MinRemainValues();
                return RecursionSolve(x, y);
            }
            else
            {
                for (int i = 1; i <= SudokuBoard.MatSize; i++)
                {
                    if (valid.IsValid(row, col, i))
                    {
                        UpdateSolver(row, col, i);
                        (x,y) = MinRemainValues();
                        if (RecursionSolve(x, y))
                            return true;
                        UpdateSolver(row, col, 0, i);
                    }

                }
                board.fails[row, col]++;
                return false;

            }


        }
        
    }
}
