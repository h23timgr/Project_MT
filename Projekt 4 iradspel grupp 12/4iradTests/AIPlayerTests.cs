using System.Drawing;
using Xunit;
using FourInARow;
using OODP;

namespace _4iradTests
{
    public class AIPlayerTests
    {
        private readonly Color aiColor = Color.Red;
        private readonly Color humanColor = Color.Yellow;

        private MinimaxAI CreateAI() => new MinimaxAI(4, aiColor, humanColor);

        [Fact]
        public void AIPlaysVaildColumn()
        {
            var board = new GameState();
            board.SetPlayerColors(aiColor, humanColor);
            var ai = CreateAI();

            int column = ai.ChooseMove(board);

            Assert.True(board.IsValidMove(column));
        }

        [Fact]
        public void AIPlaysAvailableColumn()
        {
            var board = new GameState();
            board.SetPlayerColors(aiColor, humanColor);
            // Fyll alla kolumner utom kolumn 3
            for (int col = 0; col < board.Columns; col++)
                if (col != 3)
                    FillColumn(board, col, humanColor);
            var ai = CreateAI();

            int column = ai.ChooseMove(board);

            Assert.Equal(3, column);
        }

        [Fact]
        public void AIBlocksWinningMove()
        {
            var board = new GameState();
            board.SetPlayerColors(aiColor, humanColor);

            // Fyll alla kolumner utom kolumn 2 så att de är fulla
            for (int col = 0; col < board.Columns; col++)
                if (col != 2)
                    FillColumn(board, col, aiColor);

            // Sätt tre i rad för spelaren i kolumn 2
            SetThreeInARow(board, humanColor, 2);

            var ai = CreateAI();

            int column = ai.ChooseMove(board);

            Assert.Equal(2, column);
        }

        [Fact]
        public void AIMakesWinningMove()
        {
            var board = new GameState();
            board.SetPlayerColors(aiColor, humanColor);
            // Sätt upp tre i rad för AI i kolumn 4
            SetThreeInARow(board, aiColor, 4);
            var ai = CreateAI();

            int column = ai.ChooseMove(board);

            Assert.Equal(4, column);
        }

        // Hjälpfunktioner för att fylla kolumn och sätta tre i rad
        private void FillColumn(GameState board, int col, Color color)
        {
            for (int i = 0; i < board.Rows; i++)
                board.MakeMove(col, color);
        }

        private void SetThreeInARow(GameState board, Color color, int col)
        {
            for (int i = 0; i < 3; i++)
                board.MakeMove(col, color);
        }
    }
}
