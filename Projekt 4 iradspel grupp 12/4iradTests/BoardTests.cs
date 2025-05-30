using System.Drawing;
using Xunit;
using FourInARow;

namespace _4iradTests
{
    public class BoardTests
    {
        private readonly Color player1 = Color.Red;
        private readonly Color player2 = Color.Yellow;

        [Fact]
        public void Piece_Should_Land_On_First_Available_Row()
        {
            var board = new GameState();
            board.SetPlayerColors(player1, player2);

            // Place two pieces in column 0
            board.MakeMove(0, player1);
            board.MakeMove(0, player2);

            // The bottom row is Rows-1, next is Rows-2
            Assert.Equal(player1, board.GetCell(board.Rows - 1, 0));
            Assert.Equal(player2, board.GetCell(board.Rows - 2, 0));
        }

        [Fact]
        public void Should_Not_Allow_Placement_In_Full_Column()
        {
            var board = new GameState();
            board.SetPlayerColors(player1, player2);

            // Fill column 0
            for (int i = 0; i < board.Rows; i++)
                board.MakeMove(0, player1);

            // Try to place one more
            bool result = board.MakeMove(0, player2);

            Assert.False(result);
        }

        [Fact]
        public void IsValidMove_Should_Return_False_If_Column_Is_Full()
        {
            var board = new GameState();
            for (int i = 0; i < board.Rows; i++)
                board.MakeMove(1, player1);

            Assert.False(board.IsValidMove(1));
        }

        [Fact]
        public void IsValidMove_Should_Return_True_If_Column_Is_Empty()
        {
            var board = new GameState();
            Assert.True(board.IsValidMove(2));
        }

        [Fact]
        public void ResetBoard_Should_Clear_The_Board()
        {
            var board = new GameState();
            board.MakeMove(0, player1);
            board.MakeMove(1, player2);

            board.ResetBoard();

            for (int row = 0; row < board.Rows; row++)
                for (int col = 0; col < board.Columns; col++)
                    Assert.Equal(Color.Empty, board.GetCell(row, col));
        }

        [Fact]
        public void GetCell_Should_Return_Correct_Piece()
        {
            var board = new GameState();
            board.MakeMove(3, player2);

            Assert.Equal(player2, board.GetCell(board.Rows - 1, 3));
        }

        [Fact]
        public void Color_Selection_On_Board()
        {
            var board = new GameState();
            board.SetPlayerColors(player1, player2);

            // No direct getter, but you can test by making moves and checking cell color
            board.MakeMove(0, player1);
            Assert.Equal(player1, board.GetCell(board.Rows - 1, 0));
        }
    }
}
