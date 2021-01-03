using System.Windows.Media;

namespace UI.Theme
{
    public interface ITheme
    {
	    public static ITheme Default = new DarkTheme();

        Color RulesActive { get; }
        Color RulesInactive { get; }

        Color Background { get; }
    }
}
