using FourInARow;
using OODP;
using OODP.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;

public class GameEngine // Klass för Spelmotorn
{
    private readonly IAIPlayer aiPlayer;
    private readonly ISoundPlayer soundPlayer;
    private readonly IColorManager colorManager;
    private readonly IUserInterface userInterface;
    private readonly IGameSaveService saveService;
    private bool isSinglePlayer;
    private bool isPlayer1Turn = true;
    private Stack<Tuple<int, int, bool>> moveHistory = new Stack<Tuple<int, int, bool>>();

    public GameState GameState { get; private set; } // Egenskap för spelets tillstånd
    public event Action<int, int, Color> MoveMade;

    public GameEngine(IAIPlayer aiPlayer, ISoundPlayer soundPlayer, IColorManager colorManager,
        bool isSinglePlayer, IUserInterface userInterface, IGameSaveService saveService) // Konstruktor
    {
        this.aiPlayer = aiPlayer;
        this.soundPlayer = soundPlayer; // Använd det som skickas in!
        this.colorManager = colorManager;
        this.isSinglePlayer = isSinglePlayer;
        this.userInterface = userInterface;
        this.saveService = saveService;
        GameState = new GameState();
    }

    public void MakeMove(int row, int col, bool isPlayer1, bool isLoadedGame = false) // Metod för att göra ett drag
    {
        if (!GameState.IsValidMove(col))
        {
            userInterface.ShowMessage("Ogiltigt drag! kolumnen är full ");
            return;
        }

        int actualRow = GameState.Rows - GameState.GetColumnHeight(col) - 1;
        Color color = isPlayer1Turn ? colorManager.Player1Color : colorManager.Player2Color;
        GameState.MakeMove(col, color);
        MoveMade?.Invoke(actualRow, col, color);

        moveHistory.Push(new Tuple<int, int, bool>(actualRow, col, isPlayer1Turn));

        if (GameState.CheckForWin(color))
        {
            soundPlayer.PlayWinSound();
            userInterface.ShowMessage($"{(isPlayer1Turn ? "Spelare 1" : "Spelare 2")} Vinner!");

            // Återställ hela brädet efter vinst
            ResetGame((r, c, bg) => MoveMade?.Invoke(r, c, bg)); // Skickar MoveMade för att återställa UI
        }
        else if (GameState.IsBoardFull())
        {
            userInterface.ShowMessage("Oavgjort!");

            // Återställ brädet vid oavgjort
            ResetGame((r, c, bg) => MoveMade?.Invoke(r, c, bg));
        }
        else
        {
            isPlayer1Turn = !isPlayer1Turn;
            if (isSinglePlayer && !isPlayer1Turn && !isLoadedGame)
            {
                MakeAIMove();
            }
        }
    }

    private void MakeAIMove() // Metod för att låta AI göra ett drag
    {
        int col = aiPlayer.ChooseMove(GameState);
        MakeMove(0, col, false); // Kalla MakeMove med AI:s val
    }

    public void SaveGame(string filePath) // Metod för att spara spelet
    {
        saveService.SaveGame(filePath, moveHistory);
    }

    public void LoadGame(string filePath, Action<int, int, Color> updateCallback) // Metod för att ladda spelet
    {
        var moves = saveService.LoadGame(filePath);
        ResetGame(updateCallback);
        foreach (var move in moves)
        {
            MakeMove(move.Item1, move.Item2, move.Item3, isLoadedGame: true);
        }
    }

    private void ResetGame(Action<int, int, Color> updateCallback = null) // Metod för att återställa spelet
    {
        // Återställ spelets logiska tillstånd
        GameState.ResetBoard();
        moveHistory.Clear();
        isPlayer1Turn = true;

        // Återställ UI om en callback har angetts
        if (updateCallback != null)
        {
            for (int row = 0; row < GameState.Rows; row++)
            {
                for (int col = 0; col < GameState.Columns; col++)
                {
                    updateCallback(row, col, Color.Empty); // Återställ cellens färg till tom
                }
            }
        }
    }

    public void UndoMove(Action<int, int, Color> updateCallback) // Metod för att ångra drag
    {
        if (moveHistory.Count > 0)
        {
            // Ta bort det senaste draget från historiken
            var lastMove = moveHistory.Pop();
            int row = lastMove.Item1;
            int col = lastMove.Item2;

            GameState.RemoveMove(col);
            // Återställer brädet
            updateCallback(row, col, Color.Empty); // Återställ knappen till tom
            isPlayer1Turn = !isPlayer1Turn; // Återställ turordningen
        }
        else
        {
            // Ingen draghistorik att ångra
            userInterface.ShowMessage("Finns inga drag kvar att ångra.");
        }
    }
}
