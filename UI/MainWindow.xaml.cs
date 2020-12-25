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
		private Port _port = (Port)6672;

		private readonly IPortBlocker portBlocker = IPortBlocker.Firewall();
		private readonly ITheme _theme = new DarkTheme();

		private HotkeyManager _hotKeys;

		private bool _set = false;

		public ImageSource LockImageSource { get; set; }

		public MainWindow()
		{
			InitializeComponent();
			Loaded += MainWindow_Loaded;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			Init();
		}

		void Init()
		{
			Background = _theme.Background.ToBrush();

			// rules are default disabled
			FirewallButton.Background = _theme.RulesInactive.ToBrush();
		}

		public void RulesButtonClickHandler(object sender, RoutedEventArgs e)
		{
			SetRules();
		}

		private void SetRules()
		{
			try
			{
				if (!_set)
				{
					portBlocker.Block(_port);

					_set = true;
					UpdateActive();
				}
				else
				{
					portBlocker.Unblock(_port);

					_set = false;
					UpdateNotActive();
				}
			}
			catch (Exception e)
			{
				AdminLabel.Visibility = Visibility.Visible;
				Console.Write(e);
			}
		}

		private void UpdateNotActive()
		{
			FirewallButton.Background = _theme.RulesInactive.ToBrush();
			LockImage.Source = new BitmapImage(new Uri("/Assets/unlocked.png", UriKind.Relative));

			LockLabel.Content = "Rules not active." + Environment.NewLine + "Click to activate!";
		}

		private void UpdateActive()
		{
			FirewallButton.Background = _theme.RulesActive.ToBrush();
			LockImage.Source = new BitmapImage(new Uri("/Assets/locked.png", UriKind.Relative));
			LockLabel.Content = "Rules active." + Environment.NewLine + "Click to deactivate!";
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);

			_hotKeys = HotkeyManager.Current;

			_hotKeys.AddOrReplace("rules", Key.F10, ModifierKeys.Control, (sender, args) =>
			{
				SetRules();
			});

			// delete the rules at startup to have a clean slate
			portBlocker.ReleaseAll();
		}

		protected override void OnClosed(EventArgs e)
		{
			// delete the rules at closing to not have rules be in the firewall
			portBlocker.ReleaseAll();
			base.OnClosed(e);
		}
	}
}
