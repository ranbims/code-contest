using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace CodeContest
{
    public class ReplacableCotentColorPalette
    {
        private readonly Color[] predifinedColors = { Colors.Red, Colors.Blue, Colors.DarkGoldenrod, Colors.DarkKhaki, Colors.DarkSalmon, Colors.Green, Colors.Purple };
        private Color[] randomlizedColors;
        private int index = 0;
        private Dictionary<string, Color> colorMap = new Dictionary<string, Color>();

        public ReplacableCotentColorPalette()
        {
            reset();
        }

        public void reset()
        {
            Random random = new Random();
            randomlizedColors = predifinedColors.OrderBy(x => random.Next()).ToArray();
            index = 0;
            colorMap.Clear();
        }

        public Color getColorByContent(String content)
        {
            if (!colorMap.ContainsKey(content))
            {
                colorMap[content] = randomlizedColors[index++];
            }
            return colorMap[content];
        }
    }
}
