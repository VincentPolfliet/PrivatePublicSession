
using System.Collections.Generic;
using WindowsFirewallHelper;
using Core.Logger;
using Serilog;


namespace Core.Network
{
	public class FirewallPortBlocker : IPortBlocker
	{
		private ILogger _logger = LoggerHolder.Logger;

		private const FirewallProfiles All = FirewallProfiles.Public | FirewallProfiles.Domain |
		                                     FirewallProfiles.Private;

		private const string InboundName = "[GTAO] Private Public Lobby - Inbound";
		private const string OutboundName = "[GTAO] Private Public Lobby - Outbound";

		private readonly IFirewall firewallBlocker;

		private readonly Dictionary<Port, PortRules> _rules = new();

		public FirewallPortBlocker()
		{
			firewallBlocker = FirewallManager.Instance;
		}

		public void Block(Port port)
		{
			_logger.Information("blocking port: {port}", port.Number);

			// already created these rules? just enable them
			if (_rules.ContainsKey(port))
			{
				_rules[port].Enable(true);
			}
			else
			{
				var rule = CreatePortRules(port);

				_rules.Add(port, rule);

				firewallBlocker.Rules.Add(rule.Inbound);
				firewallBlocker.Rules.Add(rule.Outbound);
			}
		}

		private PortRules CreatePortRules(in Port port) =>
			new(
				CreatePortRule(port, InboundName, FirewallDirection.Inbound),
				CreatePortRule(port, OutboundName, FirewallDirection.Outbound)
			);

		private IRule CreatePortRule(in Port port, string name, FirewallDirection direction)
		{
			var rule = firewallBlocker.CreatePortRule(
				All, name,
				FirewallAction.Block, port.Number,
				FirewallProtocol.UDP);
			rule.Direction = direction;
			return rule;
		}

		public void Unblock(Port port)
		{
			// check if the rules exist
			if (!_rules.ContainsKey(port))
			{
				// nothing to unblock if the port isn't blocked
				return;
			}

			_rules[port].Enable(false);
		}

		public void ReleaseAll()
		{
			foreach (var (port, rule) in _rules)
			{
				var (inbound, outbound) = rule;

				firewallBlocker.Rules.Remove(inbound);
				firewallBlocker.Rules.Remove(outbound);
			}
		}
	}
}
