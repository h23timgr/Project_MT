using System;
using System.Drawing;
using Xunit;

namespace _4iradTests
{
    public class PlayerTests
    {
        [Fact]
        public void ConstructorSetDefaultColors()
        {
            var colorManager = new ColorManager();

            Assert.Equal(Color.Red, colorManager.Player1Color);
            Assert.Equal(Color.Yellow, colorManager.Player2Color);
            Assert.Equal(Color.White, colorManager.BoardColor);
        }

        [Fact]
        public void ChooseColorsSetDifferentColors()
        {
            var colorManager = new ColorManager();

            Color selector(string prompt, Color defaultColor)
            {
                if (prompt.Contains("Spelare1"))
                    return Color.Red;
                if (prompt.Contains("Spelare2"))
                    return Color.Yellow;
                if (prompt.Contains("Spelbrädet"))
                    return Color.White;
                return defaultColor;
            }

            colorManager.ChooseColors(selector);

            Assert.NotEqual(colorManager.Player1Color, colorManager.Player2Color);
            Assert.Equal(Color.White, colorManager.BoardColor);
        }

        [Fact]
        public void ChooseColorsThrowExceptionSameColorsSelected()
        {
            var colorManager = new ColorManager();

            Color selector(string prompt, Color defaultColor)
            {
                return Color.Red; // Välj samma färg för båda spelarna
            }

            var exception = Assert.Throws<InvalidOperationException>(() => colorManager.ChooseColors(selector));
            Assert.Equal("Spelare 1 och Spelare 2 kan inte ha samma färg.", exception.Message);
        }
    }
}
