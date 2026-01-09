using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{
    internal class Validation
    {
        private int size = Program.MaxSize;
        private int[][] mat;

        public Validation(int[][] board)
        {
            this.mat = board;
        }
        private bool inRow(int num, int row)
        {
            for (int i = 0; i < this.mat.GetLength(0); i++)
                if (this.mat[row][i] == num)
                    return false;
            return true;
        }
        private bool inCol(int num, int col)
        {
            for (int i = 0; i < this.mat[1].GetLength(1); i++)
                if (this.mat[i][col] == num))
                        return false;
            return true;
        }


    }
}
