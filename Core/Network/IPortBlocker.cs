using System.Collections.Generic;

namespace Core.Network
{
	public interface IPortBlocker
	{
		void Block(Port port);
		void Unblock(Port port);

		BlockedPortStatus IsBlocked(Port port);

		void ReleasePort(Port port);

		public static IPortBlocker Firewall() => new FirewallPortBlocker();

		public static IPortBlocker Dev() => new DevPortBlocker();
	}
}
