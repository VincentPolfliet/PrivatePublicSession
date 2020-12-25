using System;

namespace UI.Keyboard
{
    [Flags]
    public enum HotKeyAttribute : uint
    {
        HoldAlt = 0x0001,
        HoldControl = 0x0002,
        HoldShift = 0x0004
    }
}