using System.Windows.Media;
using UI.Annotations;

namespace UI.Theme
{
	public static class HexColor
	{
		public static Color? Parse([CanBeNull] string hexString)
		{
			return hexString == null ? null : (Color?) ColorConverter.ConvertFromString(hexString);
		}
	}
}
