Welcome to my sudoku solver!
this project was done to solve any valid-solvable sudoku board, in under 1 second.
The algorithm is built on "back-tracking" algorithm for sudoku board solving.
Because back-tracking algorithm not very fast, improvements were needed. for improvements I did:

- Bit-Masking: represented each row column and box as a mask of 9 bits, each bit represents a number.
if a bit is 1, that means the number appears in the row/col/box. if not it is 0.
- MRV (Minimum remaining values): each try of the sudoku isn't done on the next cell like normal backtracking,
it's done on the cell with the minimum remaining values.
- Naked-Singles: Everytime a new cell is filled, the program searches a cell that only has
a single value option left. If one is found, the program fills it.
- Hidden-Singles: Everytime a new cell is filled, the program searches a cell, that because of its
neighboors (in his row/col/box), there is a value that only it can store. If one is found, the program fills it.


the program has 5 classes:

Program.cs - output orders, inputs boards to solve, than outputs solved board or reason it wasn't solved. In a loop
SudokuBoard.cs - represents a board. Handles going from string to board, board to string, and printing boards.

Validation.cs - handles the bit-masking. contains the function to get a cell's mask, to go from 
number to mask, mask to numer, and more.

SudokuExceptions.cs - contains the sudoku exceptions.

And the Solver.cs:
The Solver.cs conatins the Solve() function, which starts the first call to the main function: RecursionSolve().
RecursionSolve() is the back-tracking function. every call, it uses ProcessSingles() to fill all of the
naked and hidden singles, uses MRV() to find the next empty cell, fills this cell and re-call itself.
Everytime the function gets to a dead-end, it back-track, remove all the values it fills, and fills the next value.
