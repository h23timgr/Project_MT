namespace FourInARow
{
    public interface IAIPlayer // Gr�nssnitt f�r AI-spelare.
    {
        int ChooseMove(GameState gameState);
    }
}