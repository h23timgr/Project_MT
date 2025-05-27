using System;
using System.Drawing;

namespace FourInARow
{
    public class GameState // Klass f�r att hantera spelets tillst�nd
    {
        public int Rows { get; private set; } = 6;
        public int Columns { get; private set; } = 7;
        private readonly Color[,] board;
        private readonly int[] columnHeight;
        private Color aiColor;
        private Color humanColor;

        public GameState() // Konstruktor f�r att skapa ett nytt GameState-objekt
        {
            board = new Color[Rows, Columns];
            columnHeight = new int[Columns];
        }

        public GameState(GameState other) // Kopieringskonstruktor som klonar ett GameState-objekt
        {
            Rows = other.Rows;
            Columns = other.Columns;
            board = new Color[Rows, Columns];
            columnHeight = (int[])other.columnHeight.Clone();
            Array.Copy(other.board, board, other.board.Length);
            aiColor = other.aiColor;
            humanColor = other.humanColor;
        }

        public GameState Clone() // Metod f�r att skapa en kopia av ett GameState-objekt
        {
            return new GameState(this);
        }

        public bool IsValidMove(int col) // Metod f�r att kontrollera om ett drag �r giltigt
        {
            return columnHeight[col] < Rows;
        }

        public bool MakeMove(int col, Color color) // Metod f�r att g�ra ett drag
        {
            if (!IsValidMove(col))
                return false;

            int row = Rows - columnHeight[col] - 1;
            board[row, col] = color;
            columnHeight[col]++;
            return true;
        }

        public bool IsTerminal() // Metod f�r att kontrollera om spelet �r �ver
        {
            return CheckForWin(aiColor) || CheckForWin(humanColor) || IsBoardFull();
        }

        public bool IsBoardFull() // Metod f�r att kontrollera om br�det �r fullt
        {
            for (int col = 0; col < Columns; col++)
            {
                if (columnHeight[col] < Rows)
                    return false;
            }
            return true;
        }

        public bool CheckForWin(Color color) // Metod f�r att kontrollera om en spelare har vunnit
        {
            // Horisontell vinstkontroll
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns - 3; col++)
                {
                    if (board[row, col] == color &&
                        board[row, col + 1] == color &&
                        board[row, col + 2] == color &&
                        board[row, col + 3] == color)
                        return true;
                }
            }

            // Vertikal vinstkontroll
            for (int col = 0; col < Columns; col++)
            {
                for (int row = 0; row < Rows - 3; row++)
                {
                    if (board[row, col] == color &&
                        board[row + 1, col] == color &&
                        board[row + 2, col] == color &&
                        board[row + 3, col] == color)
                        return true;
                }
            }

            // Diagonal (\) vinstkontroll
            for (int row = 0; row < Rows - 3; row++)
            {
                for (int col = 0; col < Columns - 3; col++)
                {
                    if (board[row, col] == color &&
                        board[row + 1, col + 1] == color &&
                        board[row + 2, col + 2] == color &&
                        board[row + 3, col + 3] == color)
                        return true;
                }
            }

            // Diagonal (/) vinstkontroll
            for (int row = 3; row < Rows; row++)
            {
                for (int col = 0; col < Columns - 3; col++)
                {
                    if (board[row, col] == color &&
                        board[row - 1, col + 1] == color &&
                        board[row - 2, col + 2] == color &&
                        board[row - 3, col + 3] == color)
                        return true;
                }
            }

            return false;
        }

        public Color GetCell(int row, int col) // Metod f�r att h�mta f�rgen p� en specifik cell
        {
            return board[row, col];
        }

        public void SetPlayerColors(Color aiColor, Color humanColor) // Metod f�r att s�tta f�rger f�r AI och m�nniska
        {
            this.aiColor = aiColor;
            this.humanColor = humanColor;
        }

        public bool RemoveMove(int col) // Metod f�r att ta bort ett drag
        {
            if (columnHeight[col] == 0)
                return false;

            columnHeight[col]--;
            int row = Rows - columnHeight[col] - 1;
            board[row, col] = Color.White;
            return true;
        }

        public int GetColumnHeight(int col) // Metod f�r att h�mta h�jden p� en kolumn
        {
            return columnHeight[col];
        }
        public void ResetBoard() // Metod f�r att �terst�lla spelbr�det
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    board[row, col] = Color.Empty;
                }
            }

            for (int col = 0; col < Columns; col++)
            {
                columnHeight[col] = 0;
            }
        }
    }
}