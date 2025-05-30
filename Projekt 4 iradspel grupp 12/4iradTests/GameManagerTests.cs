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
        private readonly IAIPlayer dummyAI = new DummyAIPlayer();
        private readonly ISoundPlayer dummySoundPlayer = new DummySoundPlayer();
        private readonly IColorManager dummyColorManager = new ColorManager();
        private readonly IUserInterface dummyUserInterface = new DummyUserInterface();
        private readonly IGameSaveService dummyGameSaveService = new DummyGameSaveService();

        private GameEngine CreateGameEngine()
        {
            return new GameEngine(
                dummyAI,
                dummySoundPlayer,
                dummyColorManager,
                false,
                dummyUserInterface,
                dummyGameSaveService
            );
        }

        [Fact]
        public void HorizontalWin_ShouldReturnTrue()
        {
            var gameEngine = CreateGameEngine();
            int row = 0;
            bool isPlayer1 = true;

            for (int col = 0; col < 4; col++)
            {
                gameEngine.MakeMove(row, col, isPlayer1);
            }

            bool hasWon = gameEngine.GameState.CheckForWin(dummyColorManager.Player1Color);
            Assert.True(hasWon, "Fyra i rad horisontellt ska ge vinst.");
        }

        [Fact]
        public void VerticalWin_ShouldReturnTrue()
        {
            var gameEngine = CreateGameEngine();
            int col = 0;
            bool isPlayer1 = true;

            for (int row = 0; row < 4; row++)
            {
                gameEngine.MakeMove(row, col, isPlayer1);
            }

            bool hasWon = gameEngine.GameState.CheckForWin(dummyColorManager.Player1Color);
            Assert.True(hasWon, "Fyra i rad vertikalt ska ge vinst.");
        }

        [Fact]
        public void DiagonalWin_LeftToRight_ShouldReturnTrue()
        {
            var gameEngine = CreateGameEngine();
            bool isPlayer1 = true;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    gameEngine.MakeMove(j, i, !isPlayer1);
                }
                gameEngine.MakeMove(i, i, isPlayer1);
            }

            bool hasWon = gameEngine.GameState.CheckForWin(dummyColorManager.Player1Color);
            Assert.True(hasWon, "Fyra i rad diagonalt (\\) ska ge vinst.");
        }

        [Fact]
        public void DiagonalWin_RightToLeft_ShouldReturnTrue()
        {
            var gameEngine = CreateGameEngine();
            bool isPlayer1 = true;
            int maxCol = gameEngine.GameState.Columns - 1;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    gameEngine.MakeMove(j, maxCol - i, !isPlayer1);
                }
                gameEngine.MakeMove(i, maxCol - i, isPlayer1);
            }

            bool hasWon = gameEngine.GameState.CheckForWin(dummyColorManager.Player1Color);
            Assert.True(hasWon, "Fyra i rad diagonalt (/) ska ge vinst.");
        }

        [Fact]
        public void FullBoardWithoutWinner_ShouldReturnDraw()
        {
            var gameEngine = CreateGameEngine();
            int rows = gameEngine.GameState.Rows;
            int cols = gameEngine.GameState.Columns;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    bool isPlayer1 = (row + col) % 2 == 0;
                    gameEngine.MakeMove(row, col, isPlayer1);
                }
            }

            bool isDraw = gameEngine.GameState.IsBoardFull() &&
                          !gameEngine.GameState.CheckForWin(dummyColorManager.Player1Color) &&
                          !gameEngine.GameState.CheckForWin(dummyColorManager.Player2Color);

            Assert.True(isDraw, "Fullt bräde utan vinnare ska ge oavgjort.");
        }

        [Fact]
        public void Turn_ShouldSwitch_AfterMove()
        {
            var gameEngine = CreateGameEngine();
            bool moveAccepted = true;

            try
            {
                gameEngine.MakeMove(0, 0, true);  // Spelare 1 gör drag
                gameEngine.MakeMove(1, 0, true);  // Försök igen med samma spelare
            }
            catch
            {
                moveAccepted = false;
            }

            Assert.False(moveAccepted, "Efter ett drag ska turen växla och samma spelare ska inte kunna spela igen direkt.");
        }
    }

    // Dummy-klasser för beroenden:
    public class DummyAIPlayer : IAIPlayer
    {
        public int ChooseMove(GameState gameState) => 0;
    }

    public class DummySoundPlayer : ISoundPlayer
    {
        public void PlayWinSound() { }
    }

    public class DummyUserInterface : IUserInterface
    {
        public void ShowMessage(string message) { }
    }

    public class DummyGameSaveService : IGameSaveService
    {
        public void SaveGame(string filePath, Stack<Tuple<int, int, bool>> moveHistory) { }
        public List<Tuple<int, int, bool>> LoadGame(string filePath) => new();
    }
}


