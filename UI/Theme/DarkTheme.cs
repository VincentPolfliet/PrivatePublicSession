using System.Windows.Media;

namespace UI.Theme
{
    public class DarkTheme : ITheme
    {
        public Color RulesInactive { get; } = Color.FromRgb( 209,  103,  90);
        public Color RulesActive { get; } = Color.FromRgb(57, 204, 143);
        public Color Background { get; } = Color.FromRgb(50, 50, 50);
    }
}