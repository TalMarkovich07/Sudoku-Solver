using Sudoku_Solver;
namespace Sudoku_Solver.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test_WrongSizeBoard()
        {
            string input = "10000600800200000000010000000700001025000300000000004000042010003007006000000000501";

            Assert.Throws<WrongLengthException>(() => new Solver(input));
        }

        [Fact]
        public void Test_WrongBoard()
        {
            string input = "556849732307612954429357168958263471263791584741584296194536827672918345835427610";

            Assert.Throws<GivenBoardIsWrongException>(() => new Solver(input));
        }

        [Fact]
        public void Test_UnsolvableBoard()
        {
            string input = "000005080000601043000000000010500000000106000300000005530000061000000004000000000";
            var solver = new Solver(input);

            Assert.Throws<UnsolvableBoardException>(() => solver.Solve());
        }
        [Fact]
        public void Test_Correct_Result()
        {
            string[] boards = { "800000000003600000070090200050007000000045700000100030001000068008500010090000400", "100007090030020008009600500005300900010080002600004000300000010041000007007000300", "009065430007000800600108020003090002501403960804000100030509007056080000070240090" };
            string[] expected_results = { "812753649943682175675491283154237896369845721287169534521974368438526917796318452", "162857493534129678789643521475312986913586742628794135356478219241935867897261354", "289765431317924856645138729763891542521473968894652173432519687956387214178246395" };
            string[] actual_results = { "", "", "" };
            for(int i = 0; i < boards.Length; i++)
            {
                var solver = new Solver(boards[i]);

                solver.Solve();
                actual_results[i] = solver.board.BoardToString();
            }
            Assert.Equal(expected_results, actual_results);
        }
        
        [Fact]
        public void Solve_50kBoards_Under_1_Second()
        {
            string[] sudokus = File.ReadAllLines("17_clue.txt");
            int expectedBoardsOver1Second = 0;
            int actualBoardsOver1Second = 0;

            for (int i = 0; i < sudokus.Length; i++)
            {
                Solver solver = new Solver(sudokus[i]);
                solver.Solve();
                if (solver.SolveTimeMs > 1000)
                    actualBoardsOver1Second++;
            }

            Assert.Equal(actualBoardsOver1Second, expectedBoardsOver1Second);
        }
       

    }
}