using WindowsFirewallHelper;

namespace Core.Network
{
	internal class PortRules
	{
		public IRule Inbound { get; }
		public IRule Outbound { get; }

		public PortRules(IRule inbound, IRule outbound)
		{
			Inbound = inbound;
			Outbound = outbound;
		}

		public void Enable(bool enabled)
		{
			Inbound.IsEnable = enabled;
			Outbound.IsEnable = enabled;
		}

		public void Deconstruct(out IRule inbound, out IRule outbound)
		{
			inbound = Inbound;
			outbound = Outbound;
		}
	}
}
