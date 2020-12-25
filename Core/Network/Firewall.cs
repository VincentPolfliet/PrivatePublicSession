using System;
using System.Collections.Generic;
using WindowsFirewallHelper;


namespace Core.Network
{
	public class Firewall
	{
		private const FirewallProfiles All = FirewallProfiles.Public | FirewallProfiles.Domain |
		                                     FirewallProfiles.Private;

		private const string InboundName = "[GTAO] Private Public Lobby - Inbound";
		private const string OutboundName = "[GTAO] Private Public Lobby - Outbound";

		private readonly IFirewall _firewall;

		private readonly Dictionary<Port, PortRules> _rules = new();

		public Firewall()
		{
			_firewall = FirewallManager.Instance;
		}

		public void Block(Port port)
		{
			// already created these rules? just enable them
			if (_rules.ContainsKey(port))
			{
				_rules[port].Enable(true);
			}
			else
			{
				var rule = CreatePortRules(port);

				_rules.Add(port, rule);

				_firewall.Rules.Add(rule.Inbound);
				_firewall.Rules.Add(rule.Outbound);
			}
		}

		private PortRules CreatePortRules(in Port port) =>
			new(
				CreatePortRule(port, InboundName, FirewallDirection.Inbound),
				CreatePortRule(port, OutboundName, FirewallDirection.Outbound)
			);

		private IRule CreatePortRule(in Port port, string name, FirewallDirection direction)
		{
			var rule = _firewall.CreatePortRule(
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

		public void DeleteRules()
		{
			try
			{
				foreach (var (port, rule) in _rules)
				{
					var (inbound, outbound) = rule;

					_firewall.Rules.Remove(inbound);
					_firewall.Rules.Remove(outbound);
				}
			}
			catch (Exception e)
			{
				throw new FirewallException();
			}
		}
	}
}
