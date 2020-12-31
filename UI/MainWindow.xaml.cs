using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Core.Logger;
using Core.Network;
using NHotkey.Wpf;
using Serilog;
using UI.Configuration;
using UI.Theme;

namespace UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Port _port = new Port(6672);


		private readonly ILogger _logger = LoggerHolder.Logger;

		private readonly IPortBlocker _portBlocker = IPortBlocker.Firewall();
		private readonly HotkeyManager _hotKeys = HotkeyManager.Current;

		private ITheme _theme;

		private bool _rulesActive;

		private bool RulesActive
		{
			get => _rulesActive;
			set
			{
				_rulesActive = value;
				SetRules(_rulesActive);
			}
		}

		public MainWindow()
		{
			InitializeComponent();

			DataContext = this;
		}

		protected override void OnInitialized(EventArgs args)
		{
			base.OnInitialized(args);

			// theme needs to be loaded here so that SetRules and other methods can access it without nullpointer
			_theme = ConfigHolder.Configuration.Theme;

			Background = _theme.Background.ToBrush();
			InstructionsLabel.Content = Languages.Instructions;

			// register rules hotkey to CTRL + F10
			_hotKeys.AddOrReplace("rules", Key.F10, ModifierKeys.Control, (sender, hotkeyArgs) =>
			{
				FlipRules();
			});

			try
			{
				// delete the rules at startup to have a clean slate
				_portBlocker.ReleaseAll();
			}
			catch (Exception e)
			{
				_logger.Error(e, "Something went wrong");
			}

			// set default to false as initial state
			SetRules(false);
		}

		private void RulesButtonClickHandler(object sender, RoutedEventArgs e) => FlipRules();

		private void FlipRules() => RulesActive = !RulesActive;

		private void SetRules(bool enabled)
		{
			LockLabel.Content = GetLockLabelContent(enabled);
			LockImage.Source = GetLockImageSource(enabled);
			FirewallButton.Background = GetFirewallButtonBackground(enabled).ToBrush();

			try
			{
				_portBlocker.BlockOrUnblock(_port, enabled);
			}
			catch (Exception e)
			{
				_logger.Error(e, "Something went wrong with blocking the port");
			}
		}

		private static string GetLockLabelContent(bool enable)
		{
			var (rules, action) = enable
				? (Languages.RulesActive, Languages.DeactivateRules)
				: (Languages.RulesNotActive, Languages.ActivateRules);

			return rules + Environment.NewLine + action;
		}

		private static ImageSource GetLockImageSource(bool enable)
		{
			var uri = "/Assets/" + (enable ? "locked.png" : "unlocked.png");
			return new BitmapImage(new Uri(uri, UriKind.Relative));
		}

		private Color GetFirewallButtonBackground(bool enable) => enable ? _theme.RulesActive : _theme.RulesInactive;

		protected override void OnClosed(EventArgs e)
		{
			// delete the rules at closing to not have rules be in the firewall
			_portBlocker.ReleaseAll();
			base.OnClosed(e);
		}
	}
}
