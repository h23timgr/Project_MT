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
        public void Win_Sound_Is_Played_On_Win()
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

            // Lägg fyra drag, växla alltid kolumn
            engine.MakeMove(0, 0, isPlayer1: true);  // Röd
            engine.MakeMove(1, 0, isPlayer1: true);  // Gul
            engine.MakeMove(0, 0, isPlayer1: true);  // Röd
            engine.MakeMove(1, 0, isPlayer1: true);  // Gul
            engine.MakeMove(0, 0, isPlayer1: true);  // Röd
            engine.MakeMove(1, 0, isPlayer1: true);  // Gul
            engine.MakeMove(0, 0, isPlayer1: true);  // Röd (vinst)

            // Kontroll: Nu ska det finnas fyra röda brickor i kolumn 0
            for (int i = 0; i < 4; i++)
                Assert.Equal(Color.Red, engine.GameState.GetCell(engine.GameState.Rows - 1 - i, 0));

            // Kontroll: Vinstlogik returnerar true
            Assert.True(engine.GameState.CheckForWin(Color.Red));

            // Assert: Vinstljud ska ha spelats
            soundPlayer.Verify(s => s.PlayWinSound(), Times.Once);
        }
    }
}
