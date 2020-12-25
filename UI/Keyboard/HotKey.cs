using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace UI.Keyboard
{
    public class HotKey
    {
        public Key Key { get; }

        private Action _action;

        public IEnumerable<HotKeyAttribute> Attributes { get; }

        public bool Ctrl => Attributes.Contains(HotKeyAttribute.HoldControl);
        public bool Alt => Attributes.Contains(HotKeyAttribute.HoldAlt);
        public bool Shift => Attributes.Contains(HotKeyAttribute.HoldShift);

        public HotKey(Key key, Action action, HotKeyAttribute attribute) : this(key, action, new[] {attribute})
        {
        }

        public HotKey(Key key, Action action, IEnumerable<HotKeyAttribute> attributes)
        {
            Key = key;
            _action = action;
            Attributes = attributes;
        }

        public void Press()
        {
            _action();
        }
    }
}