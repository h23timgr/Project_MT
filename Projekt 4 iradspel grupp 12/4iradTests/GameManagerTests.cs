using Xunit;
using OODP;
using OODP.Interfaces;
using FourInARow;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace _4iradTests
{
    public class GameManagerTests
    {
        private readonly Color player1 = Color.Red;
        private readonly Color player2 = Color.Yellow;

        [Fact]
        public void vertical_ShouldReturnTrue()
        {
            var board = new GameState();
            board.SetPlayerColors(player1, player2);

            board.MakeMove(0, player1);
            board.MakeMove(1, player2);
            board.MakeMove(0, player1);
            board.MakeMove(1, player2);
            board.MakeMove(0, player1);
            board.MakeMove(1, player2);
            board.MakeMove(0, player1);

            // Kolla att Player1 har vunnit
            Assert.True(board.CheckForWin(player1));
        }

        [Fact]
        public void Horizontal_ShouldReturnTrue()
        {
            var board = new GameState();
            board.SetPlayerColors(player1, player2);

            board.MakeMove(0, player1);
            board.MakeMove(0, player2);
            board.MakeMove(1, player1);
            board.MakeMove(1, player2);
            board.MakeMove(2, player1);
            board.MakeMove(2, player2);
            board.MakeMove(3, player1);

            // Kolla att Player1 har vunnit
            Assert.True(board.CheckForWin(player1));
        }

        [Fact]
        public void Diagonal_LeftToRight_ShouldReturnTrue()
        {
            var board = new GameState();
            board.SetPlayerColors(player1, player2);

            board.MakeMove(0, player1);
            board.MakeMove(1, player2);
            board.MakeMove(1, player1);
            board.MakeMove(2, player2);
            board.MakeMove(2, player1);
            board.MakeMove(3, player2);
            board.MakeMove(2, player1);
            board.MakeMove(3, player2);
            board.MakeMove(4, player1);
            board.MakeMove(3, player2);
            board.MakeMove(3, player1);

            Assert.True(board.CheckForWin(player1));
        }

        [Fact]
        public void Diagonal_RightToLeft_ShouldReturnTrue()
        {
            var board = new GameState();
            board.SetPlayerColors(player1, player2);  
            
            board.MakeMove(6, player1);           
            board.MakeMove(5, player2);
            board.MakeMove(5, player1);           
            board.MakeMove(4, player2);
            board.MakeMove(4, player1);
            board.MakeMove(3, player2);
            board.MakeMove(4, player1);
            board.MakeMove(3, player2);
            board.MakeMove(2, player1);
            board.MakeMove(3, player2);
            board.MakeMove(3, player1);

            // Kolla att Player1 har vunnit
            Assert.True(board.CheckForWin(player1));
        }

        [Fact]
        public void FullBoard_ShouldReturnDraw()
        {
            var board = new GameState();
            board.SetPlayerColors(player1, player2);

            board.MakeMove(0, player1);
            board.MakeMove(0, player2);
            board.MakeMove(0, player1);
            board.MakeMove(0, player2);
            board.MakeMove(0, player1);
            board.MakeMove(0, player2);
            board.MakeMove(1, player1);
            board.MakeMove(1, player2);
            board.MakeMove(1, player1);
            board.MakeMove(1, player2);
            board.MakeMove(1, player1);
            board.MakeMove(1, player2);
            board.MakeMove(2, player1);
            board.MakeMove(2, player2);
            board.MakeMove(2, player1);
            board.MakeMove(2, player2);
            board.MakeMove(2, player1);
            board.MakeMove(2, player2);
            board.MakeMove(4, player1);
            board.MakeMove(3, player2);
            board.MakeMove(3, player1);
            board.MakeMove(3, player2);
            board.MakeMove(3, player1);
            board.MakeMove(3, player2);
            board.MakeMove(3, player1);
            board.MakeMove(4, player2);
            board.MakeMove(4, player1);
            board.MakeMove(4, player2);
            board.MakeMove(4, player1);
            board.MakeMove(4, player2);
            board.MakeMove(5, player1);
            board.MakeMove(5, player2);
            board.MakeMove(5, player1);
            board.MakeMove(5, player2);
            board.MakeMove(5, player1);
            board.MakeMove(5, player2);
            board.MakeMove(6, player1);
            board.MakeMove(6, player2);
            board.MakeMove(6, player1);
            board.MakeMove(6, player2);
            board.MakeMove(6, player1);
            board.MakeMove(6, player2);

            bool isFull = board.IsBoardFull();
            bool hasPlayer1Win = board.CheckForWin(player1);
            bool hasPlayer2Win = board.CheckForWin(player2);

            Assert.True(isFull && !hasPlayer1Win && !hasPlayer2Win);
        }

        [Fact]
        public void Turn_ShouldAlternateBetweenPlayers()
        {
            var board = new GameState();
            board.SetPlayerColors(player1, player2);

            board.MakeMove(0, player1);
            board.MakeMove(1, player2);

            Color firstMove = board.GetCell(board.Rows - 1, 0);
            Color secondMove = board.GetCell(board.Rows - 1, 1);

            Assert.NotEqual(firstMove, secondMove);
        }

        [Fact]
        public void UndoMove_ShouldRemoveLastMove()
        {
            var board = new GameState();
            board.SetPlayerColors(player1, player2);

            board.MakeMove(0, player1);
            Assert.Equal(player1, board.GetCell(board.Rows - 1, 0));

            board.RemoveMove(0);
            Assert.Equal(Color.White, board.GetCell(board.Rows - 1, 0));
        }

        [Fact]
        public void UndoMove_ShouldShowMessage_WhenNoMovesAvailable()
        {
            // Arrange
            var gameEngine = new GameEngine(
                new DummyAIPlayer(),
                new DummySoundPlayer(),
                new DummyColorManager(),
                false,
                new DummyUserInterface(),
                new DummyGameSaveService());

            // Act
            gameEngine.UndoMove((row, col, color) => { });

            // Assert
            var dummyUI = (DummyUserInterface)gameEngine.userInterface;
            Assert.Equal("Finns inga drag kvar att ångra.", dummyUI.LastMessage);
        }

        // Dummys för beroenden:
        private class DummyAIPlayer : IAIPlayer
        {
            public int ChooseMove(GameState gameState) => 0;
        }

        private class DummySoundPlayer : ISoundPlayer
        {
            public void PlayWinSound() { }
        }

        private class DummyColorManager : IColorManager
        {
            public Color Player1Color => Color.Red;
            public Color Player2Color => Color.Yellow;
            public Color BoardColor => Color.White;

            public void ChooseColors()
            {
            }

            public Color SelectColor(string prompt, Color defaultColor)
            {
                return defaultColor;
            }
        }

        private class DummyUserInterface : IUserInterface
        {
            public string LastMessage { get; private set; }
            public void ShowMessage(string message) => LastMessage = message;
        }

        private class DummyGameSaveService : IGameSaveService
        {
            public void SaveGame(string filePath, Stack<Tuple<int, int, bool>> moveHistory) { }
            public List<Tuple<int, int, bool>> LoadGame(string filePath) => new();
        }

    }
}


