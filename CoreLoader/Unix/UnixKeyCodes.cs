using System;
using CoreLoader.Unix.Native;

namespace CoreLoader.Unix
{
    public sealed class UnixKeyCodes : IKeyCodes
    {
        private readonly IntPtr _display;

        public UnixKeyCodes(IntPtr display)
        {
            _display = display;
        }

        public uint GetKeyCode(string name)
        {
            var keysym = X11.XStringToKeysym(name);
            return X11.XKeysymToKeycode(_display, keysym);
        }

        public string GetKeyName(uint code)
        {
            var keysym = X11.XKeycodeToKeysym(_display, code, 0);
            return X11.XKeysymToString(keysym);
        }
    }
}