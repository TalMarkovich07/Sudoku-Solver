using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Sudoku_Solver
{
    internal class Validation
    {
        private int[] RowMasks;
        private int[] ColMasks;
        private int[] BoxMasks;

        public Validation(int[,] mat)
        {
            int size = SudokuBoard.MatSize;
            int boxSize = SudokuBoard.BoxSize;
            RowMasks = new int[size];
            ColMasks = new int[size];
            BoxMasks = new int[size];
            for (int row = 0; row < SudokuBoard.MatSize; row++)
            {
                for (int col = 0; col < SudokuBoard.MatSize; col++)
                {
                    int val = mat[row, col];
                    if (val > 0)
                    {
                        int mask = 1 << (val -1);
                        int boxIdx = (row / boxSize) * boxSize + (col / boxSize);

                        if ((RowMasks[row] & mask) != 0)
                            throw new SudokuExceptions($"Duplicate {val} in row {row}");

                        if ((ColMasks[col] & mask) != 0)
                            throw new SudokuExceptions($"Duplicate {val} in column {col}");

                        if ((BoxMasks[boxIdx] & mask) != 0)
                            throw new SudokuExceptions($"Duplicate {val} in box {boxIdx}");

                        RowMasks[row] |= mask;
                        ColMasks[col] |= mask;
                        BoxMasks[boxIdx] |= mask;
                    }
                }
            }

        }
        public bool IsValid(int row, int col, int num)
        {
            // if num is in the given row, or in the given column, or given box, return false. else return true
            int mask = 1 << (num -1);
            int boxIdx = (row / SudokuBoard.BoxSize) * SudokuBoard.BoxSize + (col / SudokuBoard.BoxSize);
            if (((RowMasks[row] & mask) != 0) || ((ColMasks[col] & mask) != 0) || ((BoxMasks[boxIdx] & mask) != 0))
                return false;
            return true;
        }
        
        public int CountOptions(int row, int col)
        {
            int boxIdx = (row / SudokuBoard.BoxSize) * SudokuBoard.BoxSize + (col / SudokuBoard.BoxSize);
            int blocked = RowMasks[row] | ColMasks[col] | BoxMasks[boxIdx];
            int fullMask = (1 << SudokuBoard.MatSize) - 1;
            int available = ~blocked & fullMask;
            int temp = available;
            int count = 0;
            while (temp > 0)
            {
                temp &= (temp - 1);
                count++;
            }
            return count;
        }
        public int GetAvailableMask(int row, int col) // returns a mask of the available numbers in given cell
        {
            int boxIdx = (row / SudokuBoard.BoxSize) * SudokuBoard.BoxSize + (col / SudokuBoard.BoxSize);
            int blocked = RowMasks[row] | ColMasks[col] | BoxMasks[boxIdx];
            int fullMask = (1 << SudokuBoard.MatSize) - 1;
            return ~blocked & fullMask;
        }
        public int CountOnes(int mask) // count amount of 1 bits in a mask
        {
            int count = 0;
            while(mask > 0)
            {
                mask &= (mask - 1);
                count++;
            }
            return count;
        }
        public void UpdateValid(int row, int col, int num)
        {
            int mask = 1 << (num - 1);
            int boxIdx = (row / SudokuBoard.BoxSize) * SudokuBoard.BoxSize + (col / SudokuBoard.BoxSize);
            RowMasks[row] |= mask;
            ColMasks[col] |= mask;
            BoxMasks[boxIdx] |= mask;
        }
        public void ClearValid(int row, int col, int lastNum)
        {
            int mask = 1 << (lastNum - 1);
            int boxIdx = (row / SudokuBoard.BoxSize) * SudokuBoard.BoxSize + (col / SudokuBoard.BoxSize);

            RowMasks[row] &= ~mask;
            ColMasks[col] &= ~mask;
            BoxMasks[boxIdx] &= ~mask;
        }
    }
}
