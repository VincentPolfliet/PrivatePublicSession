using System.Collections.Generic;
using System.Linq;

namespace Core.Network
{
	public class DevPortBlocker : IPortBlocker
	{
		private Dictionary<Port, bool> _ports = new Dictionary<Port, bool>();

		public void Block(Port port)
		{
			_ports[port] = true;
		}

		public void Unblock(Port port)
		{
			_ports[port] = false;
		}

		public void ReleaseAll()
		{
			foreach (var key in _ports.Keys.ToList())
			{
				_ports[key] = false;
			}
		}
	}
}
