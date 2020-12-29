using System;
using System.IO;
using Newtonsoft.Json;

namespace UI.Theme
{
	public class ThemeLoader
	{
		public ITheme Load()
		{
			// load the theme from theme.json else return the default darktheme when something goes wrong
			try
			{
				return LoadThemeFromDisk();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return new DarkTheme();
			}
		}
		private ITheme LoadThemeFromDisk()
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "theme.json");
			var theme = JsonConvert.DeserializeObject(File.ReadAllText(path), typeof(ConfigTheme));

			if (theme == null)
			{
				throw new Exception("theme is null");
			}

			return (ITheme) theme;
		}
	}
}
