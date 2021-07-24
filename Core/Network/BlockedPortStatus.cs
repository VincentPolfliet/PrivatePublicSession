using System.Collections.Generic;
using WindowsFirewallHelper;

namespace Core.Network
{
	public class BlockedPortStatus
	{
		public bool InboundBlocked { get; init; }
		public bool OutboundBlocked { get; init; }

		public bool AllBlocked => InboundBlocked && OutboundBlocked;
	}
}
