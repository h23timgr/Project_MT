using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using OODP;
using OODP.Interfaces;

public class GameSaveService : IGameSaveService // Implementerar gränssnittet IGameSaveService
{
    private readonly ILoggerService logger;

    public GameSaveService(ILoggerService logger) // Konstruktor som tar ett ILoggerService-objekt som parameter
    {
        this.logger = logger;
    }

    public void SaveGame(string filePath, Stack<Tuple<int, int, bool>> moveHistory) // Sparar spelet till en fil
    {
        try
        {
            var moves = new List<Tuple<int, int, bool>>(moveHistory);
            moves.Reverse();
            var gameData = JsonConvert.SerializeObject(moves);
            File.WriteAllText(filePath, gameData);
            logger.Log($"Filen sparades i: {filePath}");
        }
        catch (Exception ex)
        {
            logger.Log($"Error i spelsparning: {ex.Message}");
        }
    }

    public List<Tuple<int, int, bool>> LoadGame(string filePath) // Laddar spelet från en fil
    {
        try
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("Filen hittades ej.");
            string gameData = File.ReadAllText(filePath);
            var moves = JsonConvert.DeserializeObject<List<Tuple<int, int, bool>>>(gameData);
            if (moves == null) throw new InvalidDataException("Ogiltigt format.");
            return moves;
        }
        catch (Exception ex)
        {
            logger.Log($"Error för att ladda spel: {ex.Message}");
            return new List<Tuple<int, int, bool>>();
        }
    }
}