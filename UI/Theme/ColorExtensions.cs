using System.Windows.Media;

namespace UI.Theme
{
    public static class ColorExtensions
    {
        public static SolidColorBrush ToBrush(this Color color)
        {
            return new SolidColorBrush(color);
        }
    }
}