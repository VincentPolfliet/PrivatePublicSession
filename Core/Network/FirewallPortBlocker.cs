using System.Collections.Generic;
using System.Linq;
using WindowsFirewallHelper;

namespace Core.Network
{
	public class FirewallPortBlocker : IPortBlocker
	{
		private const FirewallProfiles All = FirewallProfiles.Public | FirewallProfiles.Domain |
		                                     FirewallProfiles.Private;

		private const string Name = "[GTAO] Private Public Lobby - ";

		private readonly IFirewall firewall;

		public FirewallPortBlocker()
		{
			firewall = FirewallManager.Instance;
		}

		public void Block(Port port)
		{
			Block(port, FirewallDirection.Inbound);
			Block(port, FirewallDirection.Outbound);
		}

		private void Block(Port port, FirewallDirection direction)
		{
			var rule = GetRuleFromFireWall(port, direction);

			// does the rule already exists from a previous blocking?
			if (rule != null)
			{
				// if so than just enable it again
				rule.IsEnable = true;
			}
			else
			{
				// if not, create it and add it to the firewall
				var newRule = CreatePortRule(port, Name + direction, direction);
				firewall.Rules.Add(newRule);
			}
		}

		public BlockedPortStatus IsBlocked(Port port)
		{
			return new BlockedPortStatus()
			{
				InboundBlocked = IsBlockedAndEnabled(port, FirewallDirection.Inbound),
				OutboundBlocked = IsBlockedAndEnabled(port, FirewallDirection.Outbound)
			};
		}

		private bool IsBlockedAndEnabled(Port port, FirewallDirection direction)
		{
			var rule = GetRuleFromFireWall(port, direction);

			if (rule == null)
			{
				return false;
			}

			return rule.IsBlocked() && rule.IsEnable;
		}

		private IRule? GetRuleFromFireWall(Port port, FirewallDirection direction)
		{
			return firewall.Rules.FirstOrDefault(r => r.IsForPort(port) && r.IsDirection(direction));
		}

		private IRule CreatePortRule(in Port port, string name, FirewallDirection direction)
		{
			var rule = firewall.CreatePortRule(
				All, name,
				FirewallAction.Block, port.Number,
				FirewallProtocol.UDP);
			rule.Direction = direction;
			return rule;
		}

		public void Unblock(Port port)
		{
			var inboundRule = GetRuleFromFireWall(port, FirewallDirection.Inbound);

			if (inboundRule is {IsEnable: true})
			{
				inboundRule.IsEnable = false;
			}

			var outboundRule = GetRuleFromFireWall(port, FirewallDirection.Outbound);
			if (outboundRule is {IsEnable: true})
			{
				outboundRule.IsEnable = false;
			}
		}

		public void ReleasePort(Port port)
		{
			var inboundRule = GetRuleFromFireWall(port, FirewallDirection.Inbound);
			var outboundRule = GetRuleFromFireWall(port, FirewallDirection.Outbound);

			if (inboundRule is not null)
			{
				firewall.Rules.Remove(inboundRule);
			}

			if (outboundRule is not null)
			{
				firewall.Rules.Remove(outboundRule);
			}
		}
	}
}
