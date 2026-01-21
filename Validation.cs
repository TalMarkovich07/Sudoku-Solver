using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        int mask = 1;
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
    }
}
