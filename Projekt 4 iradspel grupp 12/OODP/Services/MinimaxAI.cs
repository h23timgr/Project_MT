using System;
using System.Collections.Generic;
using System.Drawing;

namespace FourInARow
{
    public class MinimaxAI : IAIPlayer // AI-spelare som använder Minimax-algoritmen.
    {
        private readonly int maxDepth;
        private readonly Color aiColor;
        private readonly Color humanColor;

        public MinimaxAI(int maxDepth, Color aiColor, Color humanColor) // Konstruktor för att skapa en ny MinimaxAI
        {
            this.maxDepth = maxDepth;
            this.aiColor = aiColor;
            this.humanColor = humanColor;
        }

        public int ChooseMove(GameState gameState) // Metod för att välja ett drag
        {
            int bestScore = int.MinValue;
            int bestColumn = 0;

            foreach (var col in GetValidMoves(gameState))
            {
                var newState = gameState.Clone();
                newState.MakeMove(col, aiColor);
                int score = Minimax(newState, maxDepth - 1, false, int.MinValue, int.MaxValue);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestColumn = col;
                }
            }
            return bestColumn;
        }

        private int Minimax(GameState gameState, int depth, bool isMaximizing, int alpha, int beta) // Minimax-algoritm med alfa-beta-beskärning.
        {

            if (depth == 0 || gameState.IsTerminal())
            {
                return Evaluate(gameState);
            }

            if (isMaximizing)
            {
                int maxEval = int.MinValue;
                foreach (var col in GetValidMoves(gameState))
                {
                    var newState = gameState.Clone();
                    newState.MakeMove(col, aiColor);
                    int eval = Minimax(newState, depth - 1, false, alpha, beta);
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                        break; // Beta-beskärning
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                foreach (var col in GetValidMoves(gameState))
                {
                    var newState = gameState.Clone();
                    newState.MakeMove(col, humanColor);
                    int eval = Minimax(newState, depth - 1, true, alpha, beta);
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                        break; // Alfa-beskärning
                }
                return minEval;
            }
        }

        private List<int> GetValidMoves(GameState gameState) // Metod för att hämta alla giltiga drag
        {
            var validMoves = new List<int>();
            for (int col = 0; col < gameState.Columns; col++)
            {
                if (gameState.IsValidMove(col))
                    validMoves.Add(col);
            }
            return validMoves;
        }

        private int Evaluate(GameState gameState) // Metod för att utvärdera spelets tillstånd
        {
            int score = 0;
            int centerCount = 0;
            int centerCol = gameState.Columns / 2;
            for (int row = 0; row < gameState.Rows; row++)
            {
                if (gameState.GetCell(row, centerCol) == aiColor)
                    centerCount++;
            }
            score += centerCount * 3;
            for (int row = 0; row < gameState.Rows; row++)
            {
                for (int col = 0; col < gameState.Columns - 3; col++)
                {
                    Color[] window = new Color[4];
                    for (int i = 0; i < 4; i++)
                        window[i] = gameState.GetCell(row, col + i);
                    score += EvaluateWindow(window);
                }
            }
            for (int col = 0; col < gameState.Columns; col++)
            {
                for (int row = 0; row < gameState.Rows - 3; row++)
                {
                    Color[] window = new Color[4];
                    for (int i = 0; i < 4; i++)
                        window[i] = gameState.GetCell(row + i, col);
                    score += EvaluateWindow(window);
                }
            }
            for (int row = 0; row < gameState.Rows - 3; row++)
            {
                for (int col = 0; col < gameState.Columns - 3; col++)
                {
                    Color[] window = new Color[4];
                    for (int i = 0; i < 4; i++)
                        window[i] = gameState.GetCell(row + i, col + i);
                    score += EvaluateWindow(window);
                }
            }
            for (int row = 3; row < gameState.Rows; row++)
            {
                for (int col = 0; col < gameState.Columns - 3; col++)
                {
                    Color[] window = new Color[4];
                    for (int i = 0; i < 4; i++)
                        window[i] = gameState.GetCell(row - i, col + i);
                    score += EvaluateWindow(window);
                }
            }

            return score;
        }

        private int EvaluateWindow(Color[] window) // Metod för att utvärdera ett fönster av fyra celler
        {
            int score = 0;
            int aiPieces = 0;
            int humanPieces = 0;
            int empty = 0;

            foreach (var cell in window)
            {
                if (cell == aiColor)
                    aiPieces++;
                else if (cell == humanColor)
                    humanPieces++;
                else
                    empty++;
            }

            if (aiPieces == 4)
                score += 100;
            else if (aiPieces == 3 && empty == 1)
                score += 5;
            else if (aiPieces == 2 && empty == 2)
                score += 2;

            // Poäng för att blockera motståndaren
            if (humanPieces == 3 && empty == 1)
                score -= 4;
            return score;
        }
    }
}