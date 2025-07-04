﻿using System.Drawing;
using Xunit;
using FourInARow;

namespace _4iradTests
{
    public class BoardTests
    {
        private readonly Color player1 = Color.Red;
        private readonly Color player2 = Color.Yellow;

        [Fact]
        public void PieceLandsOnFirstAvailableRow()
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
        public void ShouldNotAllowPlacementFullColumn()
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
        public void IsValidMoveReturnFalseColumnFull()
        {
            var board = new GameState();
            for (int i = 0; i < board.Rows; i++)
                board.MakeMove(1, player1);

            Assert.False(board.IsValidMove(1));
        }

        [Fact]
        public void IsValidMoveReturnTrueColumnEmpty()
        {
            var board = new GameState();
            Assert.True(board.IsValidMove(2));
        }

        [Fact]
        public void ResetBoardClearBoard()
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
        public void GetCellReturnCorrectPiece()
        {
            var board = new GameState();
            board.MakeMove(3, player2);

            Assert.Equal(player2, board.GetCell(board.Rows - 1, 3));
        }

        [Fact]
        public void ColorSelectionBoard()
        {
            var board = new GameState();
            board.SetPlayerColors(player1, player2);

            // No direct getter, but you can test by making moves and checking cell color
            board.MakeMove(0, player1);
            Assert.Equal(player1, board.GetCell(board.Rows - 1, 0));
        }
    }
}
