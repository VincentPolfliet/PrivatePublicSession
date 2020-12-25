using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace UI.Keyboard
{
    public class HotKeyRegister
    {
        private Window _window;

        private List<WindowsHotKey> _hotkeys = new List<WindowsHotKey>();

        public HotKeyRegister(Window window)
        {
            _window = window;
        }
        
        public int Register(Key key, Action action, bool ctrl = false, bool alt = false, bool shift = false)
        {
            var attributes = new List<HotKeyAttribute>();

            if (ctrl)
            {
                attributes.Add(HotKeyAttribute.HoldControl);
            }

            if (alt)
            {
                attributes.Add(HotKeyAttribute.HoldAlt);
            }

            if (shift)
            {
                attributes.Add(HotKeyAttribute.HoldShift);
            }

            return Register(new HotKey(key, action, attributes));
        }

        public int Register(Key key, Action action, params HotKeyAttribute[] attributes)
        {
            return Register(new HotKey(key, action, attributes));
        }
        
        public int Register(HotKey hotKey)
        {
            var windowsHotKey = new WindowsHotKey(_window, hotKey);
            _hotkeys.Add(windowsHotKey);

            windowsHotKey.Init();

            return windowsHotKey.Id;
        }

        public void UnregisterAll()
        {
            foreach (var hotkey in _hotkeys)
            {
                hotkey.Destroy();
            }
        }

        private static uint CalculateModifiers(IEnumerable<HotKeyAttribute> attributes)
        {
            var mod = attributes.Aggregate<HotKeyAttribute, HotKeyAttribute>(0, (current, attribute) => current | attribute);
            return (uint) mod;
        }

        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey([In] IntPtr hWnd, [In] int id, [In] uint fsModifiers, [In] uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey([In] IntPtr hWnd, [In] int id);

        private class WindowsHotKey
        {
            private static int _hotkeyCounter = 0;

            private const int WindowsHotKeyMessageId = 0x0312;

            public int Id { get; } = _hotkeyCounter++;

            private WindowInteropHelper _helper;
            private HwndSource _source;

            private HotKey _hotKey;

            public WindowsHotKey(Window window, HotKey hotKey)
            {
                _helper = new WindowInteropHelper(window);
                _source = HwndSource.FromHwnd(_helper.Handle);

                _hotKey = hotKey;
            }

            public void Init()
            {
                _source.AddHook(HwndHook);

                RegisterHotKey(_helper.Handle, Id, CalculateModifiers(_hotKey.Attributes),
                    (uint) KeyInterop.VirtualKeyFromKey(_hotKey.Key));
            }

            public void Destroy()
            {
                _source.RemoveHook(HwndHook);

                UnregisterHotKey(_helper.Handle, Id);
            }

            private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
            {
                switch (msg)
                {
                    case WindowsHotKeyMessageId:
                        var param = wParam.ToInt32();

                        if (param == Id)
                        {
                            OnHotKeyPressed();
                            handled = true;
                        }

                        break;
                }

                return IntPtr.Zero;
            }


            private void OnHotKeyPressed()
            {
                _hotKey.Press();
            }
        }
    }
}