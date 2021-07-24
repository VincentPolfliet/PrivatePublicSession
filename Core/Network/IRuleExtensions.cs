using System.Linq;
using WindowsFirewallHelper;

namespace Core.Network
{
	public static class RuleExtensions
	{
		public static bool IsForPort(this IRule rule, Port port)
		{
			var localPorts = rule.LocalPorts;
			var remotePorts = rule.RemotePorts;

			return localPorts.Contains(port.Number) || remotePorts.Contains(port.Number);
		}

		public static bool IsBlocked(this IRule rule) => rule.Action == FirewallAction.Block;

		public static bool IsDirection(this IRule rule, FirewallDirection direction) => rule.Direction == direction;
	}
}
