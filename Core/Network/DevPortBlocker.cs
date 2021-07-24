using System.Collections.Generic;
using System.Linq;
using Core.Logger;
using Serilog;

namespace Core.Network
{
	public class DevPortBlocker : IPortBlocker
	{
		private ILogger _logger = LoggerHolder.Logger;

		private Dictionary<Port, bool> _ports = new Dictionary<Port, bool>();

		public void Block(Port port)
		{
			_logger.Information("blocking port: {port}", port.Number);

			_ports[port] = true;
		}

		public void Unblock(Port port)
		{
			_logger.Information("Unblocking port: {port}", port.Number);

			_ports[port] = false;
		}

		public BlockedPortStatus IsBlocked(Port port)
		{
			var blocked = _ports.ContainsKey(port) && _ports[port];
			return new BlockedPortStatus {InboundBlocked = blocked, OutboundBlocked = blocked};
		}

		public void ReleasePort(Port port)
		{
			if (_ports.ContainsKey(port))
			{
				_ports.Remove(port);
			}
		}

		public void ReleasePort()
		{
			foreach (var key in _ports.Keys.ToList())
			{
				_ports[key] = false;
			}
		}
	}
}
