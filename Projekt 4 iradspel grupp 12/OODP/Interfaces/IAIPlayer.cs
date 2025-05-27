namespace FourInARow
{
    public interface IAIPlayer // Gränssnitt för AI-spelare.
    {
        int ChooseMove(GameState gameState);
    }
}