using System.Windows.Media;

namespace UI.Theme
{
    public interface ITheme
    {
        Color RulesActive { get; }
        Color RulesInactive { get; }

        Color Background { get; }
    }
}