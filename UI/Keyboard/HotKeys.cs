
using System.Windows;

namespace UI.Keyboard
{
    public static class HotKeys
    {
        public static HotKeyRegister Init(Window window)
        {
            return new(window);
        }
    }
}