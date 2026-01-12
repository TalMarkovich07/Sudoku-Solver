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
                    return true;
            return false;
        }
        private bool inCol(int num, int col)
        {
            for (int i = 0; i < this.mat[1].GetLength(1); i++)
                if (this.mat[i][col] == num)
                        return true;
            return false;
        }
        private bool inSquare(int num, int row, int col)
        {
            int root = (int)Math.Sqrt(size);
            int startRow = (int)(row / root) * root;
            int startCol = (int)(col / root) * root;
            for(int i = startRow; i < startRow + root; i++)
                for(int j = startCol; j < startCol + root; j++)
                    if(this.mat[i][j] == num)
                        return true;

            return false;
        }

        public bool Validate(int num, int row, int col)
        {
            return !inRow(num, row)&&!inCol(num, col)&&!inSquare(num, row, col);
        }


    }
}
