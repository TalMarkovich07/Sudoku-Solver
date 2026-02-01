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
        public long SolveTimeMs { get; private set; }
        public Solver(string input)
        {
            this.board = new SudokuBoard(input);
        }
        private void UpdateSolver(int row, int col, int num, int numToClear = 0)
        {
            if (num == 0)
            {
                board.valid.ClearValid(row, col, numToClear);
                board.UpdateBoard(row, col, 0);
            }
            else
            {
                board.UpdateBoard(row, col, num);
                board.valid.UpdateValid(row, col, num);
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
                        int options = board.valid.CountOptions(row, col);
                        if (options == 1)
                            return (row, col);
                        if (options == 0)
                            throw new SudokuExceptions("Unsolvable from this route");
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
            FillNakedSingles();
            if (RecursionSolve())
            {
                watch.Stop();
                SolveTimeMs = watch.ElapsedMilliseconds;
                //Console.WriteLine($"Solved in: {watch.Elapsed.TotalMilliseconds}ms, and {iterations} iterations.");
            }
            else
                throw new SudokuExceptions("Given board is un-solvable");
        }
        private bool RecursionSolve()
        {
            iterations++;
            int row = -1;
            int col = -1;
            int bestMask = 0;
            int minOptions = SudokuBoard.MatSize + 1;

            // MRV
            for (int r = 0; r < SudokuBoard.MatSize; r++)
            {
                for (int c = 0; c < SudokuBoard.MatSize; c++)
                {
                    if (board.mat[r, c] == 0)
                    {
                        int mask = board.valid.GetAvailableMask(r, c);
                        int count = board.valid.CountOnes(mask);
                        if (count == 0)
                            return false;
                        if (count < minOptions)
                        {
                            minOptions = count;
                            row = r;
                            col = c;
                            bestMask = mask;
                        }
                        if (minOptions == 1)
                            break;
                    }
                }
                if (minOptions == 1)
                    break;
            }
            if (row == -1)
                return true;
            for (int i = 1; i <= SudokuBoard.MatSize; i++)
            {
                if ((bestMask & (1 << (i - 1))) != 0)
                {
                    UpdateSolver(row, col, i);
                    if (RecursionSolve()) return true;
                    UpdateSolver(row, col, 0, i);
                }
            }
            board.fails[row, col]++;
            return false;

        }
        public void FillNakedSingles()
        {
            bool changed = true;
            int fullMask = (1 << SudokuBoard.MatSize) - 1;
            while (changed)
            {
                changed = false;
                for (int row = 0; row < SudokuBoard.MatSize; row++)
                    for (int col = 0; col < SudokuBoard.MatSize; col++)
                    {
                        if (board.mat[row, col] == 0)
                        {
                            int mask = board.valid.GetAvailableMask(row, col);
                            if (board.valid.CountOnes(mask) == 1)
                            {
                                int value = GetValueFromMask(mask);
                                UpdateSolver(row, col, value);
                                changed = true;
                            }

                        }
                    }
            }
        }
        private int GetValueFromMask(int mask)
        {
            int val = 1;
            while (mask > 1)
            {
                mask >>= 1;
                val++;
            }
            return val;
        }
    }
}
