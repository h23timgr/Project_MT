using System;
using System.Drawing;
using System.Windows.Forms;

namespace FourInARow
{
    static class Program // Huvudklass för applikationen
    {
        [STAThread]
        static void Main() // Huvudmetod
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string winSoundFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Vinstljud1.wav");

            // Visa en dialogruta för att välja spelläge
            DialogResult result = MessageBox.Show("Vill du spela mot datorn?", "Välj spelläge", MessageBoxButtons.YesNo);
            bool isSinglePlayer = (result == DialogResult.Yes);

            // Skapa nödvändiga tjänster
            IColorManager colorManager = new ColorManager();
            ISoundPlayer soundPlayer = new SoundPlayerService(winSoundFilePath);

            // Definiera AI och mänskliga spelarens färger
            Color aiColor = colorManager.Player1Color;
            Color humanColor = colorManager.Player2Color;

            // Skapa AI-spelare med önskat djup
            IAIPlayer aiPlayer = new MinimaxAI(maxDepth: 10, aiColor: aiColor, humanColor: humanColor); 
            // ju högre siffra destu svårare blir datorn och ju lägre desto enklare blir det

            // Skapa och initiera Form1 med nödvändiga beroenden
            Form1 form = new Form1(aiPlayer, soundPlayer, colorManager, isSinglePlayer);

            Application.Run(form);
        }
    }
}