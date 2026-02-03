using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Tal Markovich's Sudoku Solver!");
            Console.WriteLine("This board will solve and valid, solvable board, in under a second.");
            Console.WriteLine($"To start, enter a {Math.Pow(SudokuBoard.MatSize, 2)} long string to represent a {SudokuBoard.MatSize}x{SudokuBoard.MatSize} board.");
            while (true) 
            {
                try
                {
                    string input = Console.ReadLine();
                    Solver solver = new Solver(input);
                    solver.board.PrintBoard();
                    solver.Solve();
                    solver.board.PrintBoard();
                    Console.WriteLine($"Board as a string: {solver.board.BoardToString()}");
                    Console.WriteLine($"Board was solved in {(float)solver.SolveTimeMs / 1000} seconds");
                }
                catch (SudokuExceptions ex)
                {
                    Console.WriteLine($"Sudoku exception: {ex.Message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"System exception: {e.Message}");
                }
                finally
                {
                    Console.WriteLine($"\n\n\nEnter another {Math.Pow(SudokuBoard.MatSize, 2)} long string to represent a {SudokuBoard.MatSize}x{SudokuBoard.MatSize} board.");
                }

            }
            
        }
    }
}
