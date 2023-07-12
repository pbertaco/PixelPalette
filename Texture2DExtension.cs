using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelPalette
{
    static class Texture2DExtension
    {
        internal static int convert(this Texture2D self, IEnumerable<Color> colors)
        {
            int converted = 0;

            Color[] data = new Color[self.Width * self.Height];
            self.GetData(data);

            for (int i = 0; i < data.Length; i++)
            {
                Color color = data[i];

                if (color.A > 0)
                {
                    Color convertedColor = color.convert(colors);

                    if (convertedColor.R != color.R || convertedColor.G != color.G || convertedColor.B != color.B)
                    {
                        convertedColor.A = color.A;
                        data[i] = convertedColor;
                        converted++;
                    }
                }
            }

            self.SetData(data);

            return converted;
        }
    }
}
