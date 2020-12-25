using System;
using System.Collections.Generic;
using WindowsFirewallHelper;


namespace Core.Network
{
    public class Firewall
    {
        private const string Name = "[GTAO] Private Public Lobby";

        private readonly IFirewall _firewall;

        private readonly Dictionary<Port, IRule> _rules = new();

        public Firewall()
        {
            _firewall = FirewallManager.Instance;
        }

        public void Block(Port port)
        {
            // already created these rules? just enable them
            if (_rules.ContainsKey(port))
            {
                _rules[port].IsEnable = true;
            }
            else
            {
                var rule = CreatePortRule(port);

                _rules.Add(port, rule);
                _firewall.Rules.Add(rule);
            }
        }

        private IRule CreatePortRule(in Port port) =>
            _firewall.CreatePortRule(FirewallProfiles.Public, Name, FirewallAction.Block, port.Number);

        public void Unblock(Port port)
        {
            // check if the rules exist
            if (!_rules.ContainsKey(port))
            {
                // nothing to unblock if the port isn't blocked
                return;
            }

            _rules[port].IsEnable = false;
        }

        /// <summary>
        /// Removes CodeSwine Inbound & Outbound firewall rules at program startup.
        /// </summary>
        public void DeleteRules()
        {
            try
            {
                foreach (var (port, rule) in _rules)
                {
                    _firewall.Rules.Remove(rule);
                }
            }
            catch (Exception e)
            {
                throw new FirewallException();
            }
        }
    }
}
