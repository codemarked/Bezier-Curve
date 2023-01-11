using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafica1
{
    public class Utils
    {
        public static Random random = new Random();

        private static Color[] colors = new Color[] { Color.Yellow, Color.Orange, Color.Pink, Color.Magenta, Color.AliceBlue, Color.Lime, Color.Honeydew, Color.Lavender };

        public static Color getColor(int a)
        {
            try
            {
                return colors[a];
            } catch
            {
                return colors[random.Next(colors.Length)];
            }
        }
    }
}
