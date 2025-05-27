using System.Drawing;

namespace FourInARow
{
    public interface IColorManager // Gr�nssnitt f�r f�rgval
    {
        Color BoardColor { get; }
        public Color Player1Color { get;}
        public Color Player2Color { get; }
        void ChooseColors();
        Color SelectColor(string prompt, Color defaultColor);
    }
}