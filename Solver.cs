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
        public void Solve()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            if (RecursionSolve())
            {
                watch.Stop();
                SolveTimeMs = watch.ElapsedMilliseconds;
                //Console.WriteLine($"Solved in: {watch.Elapsed.TotalMilliseconds}ms, and {iterations} iterations.");
            }
            else
                throw new SudokuExceptions("Given board is un-solvable");
        }
        private void ProcessSingles(Queue<(int, int, int)> changesQ)
        {
            bool changed = true;
            while (changed)
            {
                changed = false;

                bool nakedFound = true;
                while (nakedFound)
                {
                    nakedFound = false;
                    for (int r = 0; r < SudokuBoard.MatSize; r++)
                    {
                        for (int c = 0; c < SudokuBoard.MatSize; c++)
                        {
                            if (board.mat[r, c] == 0)
                            {
                                int mask = board.valid.GetAvailableMask(r, c);
                                int count = board.valid.CountOnes(mask);
                                if (count == 0)
                                {
                                    UndoQ(changesQ);
                                    throw new SudokuExceptions();
                                }
                                if (count == 1)
                                {
                                    int val = GetValueFromMask(mask);
                                    UpdateSolver(r, c, val);
                                    changesQ.Enqueue((r, c, val));
                                    nakedFound = true;
                                    changed = true;
                                }
                            }
                        }
                    }
                }

                if (HiddenSingles(changesQ)) changed = true;
            }
        }
        private bool RecursionSolve()
        {
            Queue<(int, int, int)> localQ = new Queue<(int, int, int)> ();
            try
            {
                ProcessSingles(localQ);
            } catch (SudokuExceptions)
            { return false; }
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
            UndoQ(localQ);
            board.fails[row, col]++;
            return false;

        }
        private void UndoQ(Queue<(int, int, int)> Q)
        {
            int row, col, index;
            while (Q.Count > 0)
            {
                (row, col, index) = Q.Dequeue();
                UpdateSolver(row, col, 0, index);
            }
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


        public bool NakedSingles()
        {
            bool changed = false;
            int fullMask = (1 << SudokuBoard.MatSize) - 1;
            for (int row = 0; row < SudokuBoard.MatSize; row++)
                for (int col = 0; col < SudokuBoard.MatSize; col++)
                {
                    if (board.mat[row, col] == 0)
                    {
                        int mask = board.valid.GetAvailableMask(row, col);
                        int count = board.valid.CountOnes(mask);
                        if (count == 0) return false;
                        if (count == 1)
                        {
                            int value = GetValueFromMask(mask);
                            UpdateSolver(row, col, value);
                            changed = true;
                        }

                    }
                }
            return true;
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

        private bool CellHiddenSingles(int cellId, int choose, Queue<(int, int, int)> changesQ) // choose: 1 - row, 2- col, other - box
        {
            bool changed = false;
            for (int num = 1; num <= SudokuBoard.MatSize; num++)
            {
                int numMask = 1 << (num - 1);
                int count = 0;
                int lastRow = -1, lastCol = -1;

                for (int i = 0; i < SudokuBoard.MatSize; i++)
                {
                    int row, col;
                    if (choose == 1) { row = cellId; col = i; }
                    else if (choose == 2) { row = i; col = cellId; }
                    else
                    {
                        row = (cellId / SudokuBoard.BoxSize) * SudokuBoard.BoxSize + (i / SudokuBoard.BoxSize);
                        col = (cellId % SudokuBoard.BoxSize) * SudokuBoard.BoxSize + (i % SudokuBoard.BoxSize);
                    }
                    if (board.mat[row, col] == 0 && (board.valid.GetAvailableMask(row, col) & numMask) != 0)
                    {
                        count++;
                        lastRow = row;
                        lastCol = col;
                    }
                    if (count > 1) break;

                }
                if (count == 1)
                {
                    changesQ.Enqueue((lastRow, lastCol, num));
                    UpdateSolver(lastRow, lastCol, num);
                    changed = true;
                }
            }
            return changed;
        }
        public bool HiddenSingles(Queue<(int, int, int)> q)
        {
            bool changed = false;
            for(int i = 0; i < SudokuBoard.MatSize; i++)
            {
                changed |= CellHiddenSingles(i, 1, q);
                changed |= CellHiddenSingles(i, 2, q);
                changed |= CellHiddenSingles(i, 3, q);
            }
            return changed;
        }
    }
}
