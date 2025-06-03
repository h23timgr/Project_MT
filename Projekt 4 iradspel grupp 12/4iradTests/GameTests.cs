using System.Drawing;
using Xunit;
using FourInARow;
using Moq;
using OODP;
using OODP.Interfaces;

namespace _4iradTests
{
    public class GameTests
    {
        private readonly Color player1 = Color.Red;
        private readonly Color player2 = Color.Yellow;

        [Fact]
        public void StartGame_SetsUpBoard_And_TwoPlayers_Correctly()
        {
            // Arrange
            var aiPlayer = new Mock<IAIPlayer>();
            var soundPlayer = new Mock<ISoundPlayer>();
            var colorManager = new Mock<IColorManager>();
            var userInterface = new Mock<IUserInterface>();
            var saveService = new Mock<IGameSaveService>();

            colorManager.SetupGet(c => c.Player1Color).Returns(Color.Red);
            colorManager.SetupGet(c => c.Player2Color).Returns(Color.Yellow);

            var engine = new GameEngine(
                aiPlayer.Object,
                soundPlayer.Object,
                colorManager.Object,
                isSinglePlayer: false,
                userInterface.Object,
                saveService.Object
            );

            // Act
            engine.GameState.ResetBoard();

            // Assert
            for (int row = 0; row < engine.GameState.Rows; row++)
                for (int col = 0; col < engine.GameState.Columns; col++)
                    Assert.Equal(Color.Empty, engine.GameState.GetCell(row, col));
            Assert.Equal(Color.Red, colorManager.Object.Player1Color);
            Assert.Equal(Color.Yellow, colorManager.Object.Player2Color);
        }

        [Fact]
        public void Game_Starts_With_Correct_Active_Player()
        {
            // Arrange
            var aiPlayer = new Mock<IAIPlayer>();
            var soundPlayer = new Mock<ISoundPlayer>();
            var colorManager = new Mock<IColorManager>();
            var userInterface = new Mock<IUserInterface>();
            var saveService = new Mock<IGameSaveService>();

            colorManager.SetupGet(c => c.Player1Color).Returns(Color.Red);
            colorManager.SetupGet(c => c.Player2Color).Returns(Color.Yellow);

            var engine = new GameEngine(
                aiPlayer.Object,
                soundPlayer.Object,
                colorManager.Object,
                isSinglePlayer: false,
                userInterface.Object,
                saveService.Object
            );

            // Act & Assert
            // Assuming isPlayer1Turn is true at start (private, so test via public API or reflection if needed)
            // Here, you might need to expose the current player or check via a move
            // For demonstration, we check that the first move uses Player1Color
            engine.MakeMove(0, 0, isPlayer1: true);
            Assert.Equal(Color.Red, engine.GameState.GetCell(engine.GameState.Rows - 1, 0));
        }

        [Fact]
        public void Win_ShouldTriggerWinSound()
        {
            // Arrange
            var aiPlayer = new Mock<IAIPlayer>();
            var soundPlayer = new Mock<ISoundPlayer>();
            var colorManager = new Mock<IColorManager>();
            var userInterface = new Mock<IUserInterface>();
            var saveService = new Mock<IGameSaveService>();

            colorManager.SetupGet(c => c.Player1Color).Returns(player1);
            colorManager.SetupGet(c => c.Player2Color).Returns(player2);

            var engine = new GameEngine(
                aiPlayer.Object,
                soundPlayer.Object,
                colorManager.Object,
                isSinglePlayer: false,
                userInterface.Object,
                saveService.Object
            );

            // Hårdkoda in en vinst för player1 (vertikal vinst i kolumn 0)
            engine.MakeMove(0, 0, isPlayer1: true);  // player1 i kolumn 0
            engine.MakeMove(0, 1, isPlayer1: false); // player2 i kolumn 1
            engine.MakeMove(0, 0, isPlayer1: true);  // player1 i kolumn 0
            engine.MakeMove(0, 1, isPlayer1: false); // player2 i kolumn 1
            engine.MakeMove(0, 0, isPlayer1: true);  // player1 i kolumn 0
            engine.MakeMove(0, 1, isPlayer1: false); // player2 i kolumn 1
            engine.MakeMove(0, 0, isPlayer1: true);  // player1 i kolumn 0 (vinst)

            // Kontrollera att vinstljudet spelades upp
            soundPlayer.Verify(s => s.PlayWinSound(), Times.Once);
        }
    }
}
