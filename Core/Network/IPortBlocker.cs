using System.Collections.Generic;

namespace Core.Network
{
	public interface IPortBlocker
	{
		void Block(Port port);
		void Unblock(Port port);

		void ReleaseAll();

		public static IPortBlocker Firewall() => new FirewallPortBlocker();

		public static IPortBlocker Dev() => new DevPortBlocker();
	}
}
