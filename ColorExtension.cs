using Microsoft.Xna.Framework;

namespace PixelPalette
{
    static class ColorExtension
    {
        internal static Color convert(this Color self, IEnumerable<Color> colors)
        {
            int i = 0;
            Color color = new();
            double distance = int.MaxValue;

            foreach (Color value in colors)
            {
                double x = Math.Pow(self.R - value.R, 2) + Math.Pow(self.G - value.G, 2) + Math.Pow(self.B - value.B, 2);

                if (x < distance)
                {
                    distance = x;
                    color = value;
                }

                if (distance == 0)
                {
                    break;
                }

                i++;
            }

            return color;
        }
    }
}
