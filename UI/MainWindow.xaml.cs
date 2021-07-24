using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
using UI.Utils;

namespace UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly ILogger _logger = LoggerHolder.Logger;

		private readonly IPortBlocker _portBlocker = IPortBlocker.Firewall();
		private readonly HotkeyManager _hotKeys = HotkeyManager.Current;

		private AppConfig _config;
		private IEnumerable<Port> _ports;
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

			_config = ConfigHolder.Configuration;

			// theme needs to be loaded here so that SetRules and other methods can access it without nullpointer
			_theme = _config.Theme;

			_ports = GetPorts(_config.Ports);

			Background = _theme.Background.ToBrush();
			InstructionsLabel.Content = Languages.Instructions;

			// register rules hotkey to CTRL + F10
			_hotKeys.AddOrReplace("rules", Key.F10, ModifierKeys.Control, (sender, hotkeyArgs) =>
			{
				FlipRules();
			});

			// set default to false as initial state
			SetRulesBasedOnPortBlockedAmount();
		}

		private void SetRulesBasedOnPortBlockedAmount()
		{
			var allBlocked = true;

			foreach (var port in _ports)
			{
				var status = _portBlocker.IsBlocked(port);
				_logger.Information("{Port} inbound blocked: {Status}", port.Number, status.InboundBlocked);
				_logger.Information("{Port} outbound blocked: {Status}", port.Number, status.OutboundBlocked);

				allBlocked &= status.AllBlocked;
			}

			SetRules(allBlocked);
		}

		private IEnumerable<Port> GetPorts(IEnumerable<ushort> ports)
		{
			return ports.Select(n => new Port(n));
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
				_portBlocker.BlockOrUnblockAll(_ports, enabled);
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
			if (_config.DeletePortRulesOnExit)
			{
				foreach (var port in _ports)
				{
					_portBlocker.ReleasePort(port);
				}
			}

			base.OnClosed(e);
		}
	}
}
