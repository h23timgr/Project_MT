using System;
using System.Media;
using System.Windows.Forms;
using System.IO;
using System;
using System.Media;
using FourInARow;

public class SoundPlayerService : ISoundPlayer // Klass för ljuduppspelning
{
    private readonly SoundPlayer winSoundPlayer;

    public SoundPlayerService(string winSoundFilePath) // Konstruktor
    {
        winSoundPlayer = new SoundPlayer(winSoundFilePath);
    }

    public void PlayWinSound() // Spela upp vinnarljud
    {
        try
        {
            winSoundPlayer.Play();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error ljuduppspelning: {ex.Message}");
        }
    }
}