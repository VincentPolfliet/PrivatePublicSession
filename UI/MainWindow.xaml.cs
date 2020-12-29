using System;

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Core.Network;
using NHotkey.Wpf;

using UI.Theme;

namespace UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Port _port = new Port(6672);

		private readonly IPortBlocker portBlocker = IPortBlocker.Firewall();
		private readonly ITheme _theme = new DarkTheme();

		private HotkeyManager _hotKeys = HotkeyManager.Current;

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

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			// register rules hotkey to CTRL + F10
			_hotKeys.AddOrReplace("rules", Key.F10, ModifierKeys.Control, (sender, args) =>
			{
				FlipRules();
			});

			// delete the rules at startup to have a clean slate
			portBlocker.ReleaseAll();

			// set default to false as initial state
			SetRules(false);

			Background = _theme.Background.ToBrush();

			InstructionsLabel.Content = Languages.Instructions;
		}

		public void RulesButtonClickHandler(object sender, RoutedEventArgs e) => FlipRules();

		private void FlipRules()
		{
			RulesActive = !RulesActive;
		}

		private void SetRules(bool enabled)
		{
			try
			{
				if (enabled)
				{
					portBlocker.Block(_port);
				}
				else
				{
					portBlocker.Unblock(_port);
				}

				LockLabel.Content = GetLockLabelContent(enabled);
				LockImage.Source = GetLockImageSource(enabled);
				FirewallButton.Background = GetFirewallButtonBackground(enabled).ToBrush();
			}
			catch (Exception e)
			{
				AdminLabel.Visibility = Visibility.Visible;
				Console.Write(e);
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
			portBlocker.ReleaseAll();
			base.OnClosed(e);
		}
	}
}
