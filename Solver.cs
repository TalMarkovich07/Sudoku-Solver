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
        public void Solve()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            if (RecursionSolve(0, 0))
            {
                watch.Stop();
                Console.WriteLine($"Solved in: {watch.Elapsed.TotalMilliseconds}ms");
            }
            else
                throw new SudokuExceptions("Given board is un-solvable");
        }
        private bool RecursionSolve(int row, int col)
        {
            iterations++;
            if (row == SudokuBoard.MatSize)
                return true;
            if (col == SudokuBoard.MatSize)
                return RecursionSolve(row + 1, 0);

            if (board.mat[row, col] != 0)
                return RecursionSolve(row, col + 1);
            else
            {
                for (int i = 1; i <= SudokuBoard.MatSize; i++)
                {
                    if (valid.IsValid(row, col, i))
                    {
                        UpdateSolver(row, col, i);
                        if (RecursionSolve(row, col + 1))
                            return true;
                        UpdateSolver(row, col, 0, i);
                    }

                }
                return false;

            }


        }
        
    }
}
