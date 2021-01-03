using System.Collections.Generic;
using Core.Network;

namespace UI.Utils
{
	public static class PortBlockerExtensionMethods
	{
		internal static void BlockOrUnblockAll(this IPortBlocker blocker, IEnumerable<Port> ports, in bool enabled)
		{
			foreach (var port in ports)
			{
				blocker.BlockOrUnblock(port, enabled);
			}
		}

		internal static void BlockOrUnblock(this IPortBlocker blocker, Port port, bool block)
		{
			if (block)
			{
				blocker.Block(port);
			}
			else
			{
				blocker.Unblock(port);
			}
		}
	}
}
