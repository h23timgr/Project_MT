using FourInARow;
using System;
using System.Drawing;
using System.Windows.Forms;

public class ColorManager : IColorManager
{
    public Color Player1Color { get; private set; }
    public Color Player2Color { get; private set; }
    public Color BoardColor { get; private set; }

    public ColorManager()
    {
        Player1Color = Color.Red;
        Player2Color = Color.Yellow;
        BoardColor = Color.White;
    }

    // Produktionsversionen (med MessageBox)
    public void ChooseColors()
    {
        bool colorsAreValid = false;

        while (!colorsAreValid)
        {
            try
            {
                ChooseColors(SelectColor);
                colorsAreValid = true;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public void ChooseColors(Func<string, Color, Color> colorSelector)
    {
        Player1Color = colorSelector("Färgval för Spelare1:", Player1Color);
        Player2Color = colorSelector("Färgval för Spelare2:", Player2Color);

        if (Player1Color == Player2Color)
        {
            throw new InvalidOperationException("Spelare 1 och Spelare 2 kan inte ha samma färg.");
        }

        BoardColor = colorSelector("Färgval på Spelbrädet:", BoardColor);
    }

    public Color SelectColor(string prompt, Color defaultColor)
    {
        DialogResult result = MessageBox.Show(
            prompt + " Vill du välja en egen färg?",
            "Välj färg", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.AllowFullOpen = true;
                colorDialog.ShowHelp = true;
                colorDialog.Color = Color.White;

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    return colorDialog.Color;
                }
            }
        }

        return defaultColor;
    }
}
