using Core.Network;

namespace UI
{
	public static class PortBlockerExtensionMethods
	{
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
