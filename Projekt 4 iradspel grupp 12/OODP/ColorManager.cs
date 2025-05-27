using FourInARow;

public class ColorManager : IColorManager // Klass för färgval
{
    public Color Player1Color { get; private set; }
    public Color Player2Color { get; private set; }
    public Color BoardColor { get; private set; }

    public ColorManager() // Konstruktor
    {
        Player1Color = Color.Red;
        Player2Color = Color.Yellow;
        BoardColor = Color.White;
    }

    public void ChooseColors() // Metod för att välja färger för spelarna och spelbrädet
    {
        bool colorsAreValid = false;

        // Upprepa tills olika färger är valda för spelarna
        while (!colorsAreValid)
        {
            // Låter användaren välja färger för båda spelarna
            Player1Color = SelectColor("Färgval för Spelare1: ", Player1Color);
            Player2Color = SelectColor("Färgval för Spelare 2: ", Player2Color);

            // Kontrollera om spelarna har valt samma färg
            if (Player1Color == Player2Color)
            {
                // Visa ett felmeddelande om färgerna är desamma
                MessageBox.Show("Spelare 1 och Spelare 2 kan inte ha samma färg. Välj olika färger.",
                                "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                colorsAreValid = true; // Färgerna är giltiga om de är olika
            }
        }
        BoardColor = SelectColor("Färgval på Spelbrädet: ", BoardColor);
    }
    public Color SelectColor(string prompt, Color defaultColor) // Metod för att fråga spelaren om att välja en färg
    {
        // Fråga om användaren vill välja en egen färg eller använda en standardfärg
        DialogResult result = MessageBox.Show(prompt + " Vill du välja en egen färg?",
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

        // Om användaren väljer "No", returnera den förvalda standardfärgen
        return defaultColor;
    }
}