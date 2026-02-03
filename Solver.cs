using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{
    public class Solver
    {
        public int iterations = 0;
        public SudokuBoard board;
        public System.Diagnostics.Stopwatch watch;
        public long SolveTimeMs { get; private set; }
        public Solver(string input)
        {
            this.board = new SudokuBoard(input);
        }
        public void Solve()
        {
            watch = System.Diagnostics.Stopwatch.StartNew();
            if (RecursionSolve())
            {
                watch.Stop();
                SolveTimeMs = watch.ElapsedMilliseconds;
                //Console.WriteLine($"Solved in: {watch.Elapsed.TotalMilliseconds}ms, and {iterations} iterations.");
            }
            else
                throw new UnsolvableBoardException();
        }
        private bool RecursionSolve() // The actual BackTracking function to solve the board
        {
            if (iterations % 5000 == 0)
                if (watch.ElapsedMilliseconds > 999) // if board takes longer than 1 second, it's unsolvable.
                    return false;
            iterations++;

            Queue<(int, int, int)> localQ = new Queue<(int, int, int)>();
            //this queue will hold the values of the last changes the ProcessSingle() made. (row, column, mask of the number it added)
            if (!ProcessSingles(localQ))
            {
                //ProcessSingles() fails when the current board has an unsolvable cell. In that case we want to undo the changes and back-track
                UndoQ(localQ);
                return false;
            }
            (int row, int col, int mask) = MRV();
            if (row == -1) //if the MRV returns row = -1, the board is solved
                return true;
            if (row == SudokuBoard.MatSize) //if the MRV returns row = MatSize, the board is unsolvable needs a back-track
                return false;
            for (int i = 1; i <= SudokuBoard.MatSize; i++)
                if ((mask & (1 << (i - 1))) != 0)
                {
                    //recurse the function for every value that is possible in the location given by the MRV
                    UpdateSolver(row, col, i);
                    if (RecursionSolve()) return true;
                    UpdateSolver(row, col, 0, i);
                }
            //if no value was found, board is unsolvable and needs a back-track and an undo
            board.fails[row, col]++;
            UndoQ(localQ);
            return false;

        }
        private bool ProcessSingles(Queue<(int, int, int)> changesQ)
        {
            //function updates any hidden or naked singles, and save the changes to the given queue
            bool changed = true;
            while (changed) // if a change was found, another single might appear
            {
                changed = false;
                for (int r = 0; r < SudokuBoard.MatSize; r++)
                { // naked singles - cells that only have 1 options
                    for (int c = 0; c < SudokuBoard.MatSize; c++)
                    {
                        if (board.mat[r, c] == 0)
                        {
                            int mask = board.valid.GetAvailableMask(r, c);
                            int count = board.valid.CountOnes(mask);
                            if (count == 0) // if a cell has zero options, a back-track is needed
                            {
                                UndoQ(changesQ);
                                return false;
                            }
                            if (count == 1)
                            {
                                int val = GetValueFromMask(mask);
                                UpdateSolver(r, c, val);
                                changesQ.Enqueue((r, c, val));
                                changed = true;
                            }
                        }
                    }
                }
                if (HiddenSingles(changesQ)) changed = true;
            }
            return true;
        }
        
        private (int, int, int) MRV()
        //finds the empty cell with the least amount of options
        //returns row, col, mask of the cell's values. if board is solved, all values are -1. if board is unsolvable, all values are MatSize
        {
            int row = -1;
            int col = -1;
            int bestMask = 0;
            int minOptions = SudokuBoard.MatSize + 1;

            for (int r = 0; r < SudokuBoard.MatSize; r++)
            {
                for (int c = 0; c < SudokuBoard.MatSize; c++)
                {
                    if (board.mat[r, c] == 0)
                    {
                        int mask = board.valid.GetAvailableMask(r, c); //gets the cell's mask
                        int count = board.valid.CountOnes(mask); //count options in the cell
                        if (count == 0) //a back-track is needed
                            return (SudokuBoard.MatSize, SudokuBoard.MatSize, SudokuBoard.MatSize);
                        if ((count < minOptions) || (count == minOptions && board.fails[r, c] > board.fails[row, col]))
                        {
                            //change minOptions if there a less options, or if there are equal amount of options but more fails
                            minOptions = count;
                            row = r;
                            col = c;
                            bestMask = mask;
                        }
                    }
                }
            }
            if (row == -1)
                return (-1, -1, -1); //no values left to fill
            return (row, col, bestMask);
        }
        private void UndoQ(Queue<(int, int, int)> Q)
        { //undo every change that was pushed in the queue.
            int row, col, index;
            while (Q.Count > 0)
            {
                (row, col, index) = Q.Dequeue();
                UpdateSolver(row, col, 0, index);
            }
        }
        private void UpdateSolver(int row, int col, int num, int numToClear = 0)
        { //given a row, column, a number and a number to clear, updates valid and board as wished 
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
        
        private int GetValueFromMask(int mask)
        {
            //given a bit-mask of value, returns the value
            int val = 1;
            while (mask > 1)
            {
                mask >>= 1;
                val++;
            }
            return val;
        }

        private bool CellHiddenSingles(int cellId, int choose, Queue<(int, int, int)> changesQ) // choose: 1 - row, 2- col, other - box
        { // checks if there is a value that only has a single option in the given unit - line/row/box (depends on choose)
            bool changed = false;
            for (int num = 1; num <= SudokuBoard.MatSize; num++) //iterate over all the possible values
            {
                int numMask = 1 << (num - 1);
                int count = 0;
                int lastRow = -1, lastCol = -1;

                for (int i = 0; i < SudokuBoard.MatSize; i++) //iterates over all of the given unit
                {
                    int row, col;
                    if (choose == 1) { row = cellId; col = i; }
                    else if (choose == 2) { row = i; col = cellId; }
                    else //calculate the row and col depends on the given boxId
                    {
                        row = (cellId / SudokuBoard.BoxSize) * SudokuBoard.BoxSize + (i / SudokuBoard.BoxSize);
                        col = (cellId % SudokuBoard.BoxSize) * SudokuBoard.BoxSize + (i % SudokuBoard.BoxSize);
                    }
                    if (board.mat[row, col] == 0 && (board.valid.GetAvailableMask(row, col) & numMask) != 0)
                    { //if cell is empty and num can appear there
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
        { // do CellHiddenSingles() for every row, col and box in the mat, return true if something changes
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
