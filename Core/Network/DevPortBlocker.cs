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
			_logger.Information("UNblocking port: {port}", port.Number);

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
