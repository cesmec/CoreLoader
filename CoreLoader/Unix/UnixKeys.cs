using System;
using CoreLoader.Input;
using CoreLoader.Unix.Native;

namespace CoreLoader.Unix
{
    internal sealed class UnixKeys : IKeys
    {
        private readonly IntPtr _display;

        public UnixKeys(IntPtr display)
        {
            _display = display;

            Back = X11.XKeysymToKeycode(_display, 0xff08);
            Tab = X11.XKeysymToKeycode(_display, 0xff09);
            Return = X11.XKeysymToKeycode(_display, 0xff0d);
            Shift = X11.XKeysymToKeycode(_display, 0xffe1);
            Control = X11.XKeysymToKeycode(_display, 0xffe3);
            Menu = X11.XKeysymToKeycode(_display, 0xffe9);
            Pause = X11.XKeysymToKeycode(_display, 0xff6b);
            Capital = X11.XKeysymToKeycode(_display, 0xffe5);
            Escape = X11.XKeysymToKeycode(_display, 0xff1b);
            Space = X11.XKeysymToKeycode(_display, 0x20);
            PageUp = X11.XKeysymToKeycode(_display, 0xff55);
            PageDown = X11.XKeysymToKeycode(_display, 0xff56);
            End = X11.XKeysymToKeycode(_display, 0xff57);
            Home = X11.XKeysymToKeycode(_display, 0xff58);
            Left = X11.XKeysymToKeycode(_display, 0x08fb);
            Up = X11.XKeysymToKeycode(_display, 0x08fc);
            Right = X11.XKeysymToKeycode(_display, 0x08fd);
            Down = X11.XKeysymToKeycode(_display, 0x08fe);
            Insert = X11.XKeysymToKeycode(_display, 0xff63);
            Delete = X11.XKeysymToKeycode(_display, 0xffff);
            Help = X11.XKeysymToKeycode(_display, 0xff6a);
            Key0 = X11.XKeysymToKeycode(_display, 0x30);
            Key1 = X11.XKeysymToKeycode(_display, 0x31);
            Key2 = X11.XKeysymToKeycode(_display, 0x32);
            Key3 = X11.XKeysymToKeycode(_display, 0x33);
            Key4 = X11.XKeysymToKeycode(_display, 0x34);
            Key5 = X11.XKeysymToKeycode(_display, 0x35);
            Key6 = X11.XKeysymToKeycode(_display, 0x36);
            Key7 = X11.XKeysymToKeycode(_display, 0x37);
            Key8 = X11.XKeysymToKeycode(_display, 0x38);
            Key9 = X11.XKeysymToKeycode(_display, 0x39);
            A = X11.XKeysymToKeycode(_display, 0x41);
            B = X11.XKeysymToKeycode(_display, 0x42);
            C = X11.XKeysymToKeycode(_display, 0x43);
            D = X11.XKeysymToKeycode(_display, 0x44);
            E = X11.XKeysymToKeycode(_display, 0x45);
            F = X11.XKeysymToKeycode(_display, 0x46);
            G = X11.XKeysymToKeycode(_display, 0x47);
            H = X11.XKeysymToKeycode(_display, 0x48);
            I = X11.XKeysymToKeycode(_display, 0x49);
            J = X11.XKeysymToKeycode(_display, 0x4A);
            K = X11.XKeysymToKeycode(_display, 0x4B);
            L = X11.XKeysymToKeycode(_display, 0x4C);
            M = X11.XKeysymToKeycode(_display, 0x4D);
            N = X11.XKeysymToKeycode(_display, 0x4E);
            O = X11.XKeysymToKeycode(_display, 0x4F);
            P = X11.XKeysymToKeycode(_display, 0x50);
            Q = X11.XKeysymToKeycode(_display, 0x51);
            R = X11.XKeysymToKeycode(_display, 0x52);
            S = X11.XKeysymToKeycode(_display, 0x53);
            T = X11.XKeysymToKeycode(_display, 0x54);
            U = X11.XKeysymToKeycode(_display, 0x55);
            V = X11.XKeysymToKeycode(_display, 0x56);
            W = X11.XKeysymToKeycode(_display, 0x57);
            X = X11.XKeysymToKeycode(_display, 0x58);
            Y = X11.XKeysymToKeycode(_display, 0x59);
            Z = X11.XKeysymToKeycode(_display, 0x5A);
            Lwin = X11.XKeysymToKeycode(_display, 0xffeb);
            Rwin = X11.XKeysymToKeycode(_display, 0xffec);
            Numpad0 = X11.XKeysymToKeycode(_display, 0xffb0);
            Numpad1 = X11.XKeysymToKeycode(_display, 0xffb1);
            Numpad2 = X11.XKeysymToKeycode(_display, 0xffb2);
            Numpad3 = X11.XKeysymToKeycode(_display, 0xffb3);
            Numpad4 = X11.XKeysymToKeycode(_display, 0xffb4);
            Numpad5 = X11.XKeysymToKeycode(_display, 0xffb5);
            Numpad6 = X11.XKeysymToKeycode(_display, 0xffb6);
            Numpad7 = X11.XKeysymToKeycode(_display, 0xffb7);
            Numpad8 = X11.XKeysymToKeycode(_display, 0xffb8);
            Numpad9 = X11.XKeysymToKeycode(_display, 0xffb9);
            Multiply = X11.XKeysymToKeycode(_display, 0xffaa);
            Add = X11.XKeysymToKeycode(_display, 0xffab);
            Separator = X11.XKeysymToKeycode(_display, 0xffac);
            Subtract = X11.XKeysymToKeycode(_display, 0xffad);
            Decimal = X11.XKeysymToKeycode(_display, 0xffae);
            Divide = X11.XKeysymToKeycode(_display, 0xffaf);
            F1 = X11.XKeysymToKeycode(_display, 0xffbe);
            F2 = X11.XKeysymToKeycode(_display, 0xffbf);
            F3 = X11.XKeysymToKeycode(_display, 0xffc0);
            F4 = X11.XKeysymToKeycode(_display, 0xffc1);
            F5 = X11.XKeysymToKeycode(_display, 0xffc2);
            F6 = X11.XKeysymToKeycode(_display, 0xffc3);
            F7 = X11.XKeysymToKeycode(_display, 0xffc4);
            F8 = X11.XKeysymToKeycode(_display, 0xffc5);
            F9 = X11.XKeysymToKeycode(_display, 0xffc6);
            F10 = X11.XKeysymToKeycode(_display, 0xffc7);
            F11 = X11.XKeysymToKeycode(_display, 0xffc8);
            F12 = X11.XKeysymToKeycode(_display, 0xffc9);
            F13 = X11.XKeysymToKeycode(_display, 0xffca);
            F14 = X11.XKeysymToKeycode(_display, 0xffcb);
            F15 = X11.XKeysymToKeycode(_display, 0xffcc);
            F16 = X11.XKeysymToKeycode(_display, 0xffcd);
            F17 = X11.XKeysymToKeycode(_display, 0xffce);
            F18 = X11.XKeysymToKeycode(_display, 0xffcf);
            F19 = X11.XKeysymToKeycode(_display, 0xffd0);
            F20 = X11.XKeysymToKeycode(_display, 0xffd1);
            F21 = X11.XKeysymToKeycode(_display, 0xffd2);
            F22 = X11.XKeysymToKeycode(_display, 0xffd3);
            F23 = X11.XKeysymToKeycode(_display, 0xffd4);
            F24 = X11.XKeysymToKeycode(_display, 0xffd5);
            Lshift = X11.XKeysymToKeycode(_display, 0xffe1);
            Rshift = X11.XKeysymToKeycode(_display, 0xffe2);
            Lcontrol = X11.XKeysymToKeycode(_display, 0xffe3);
            Rcontrol = X11.XKeysymToKeycode(_display, 0xffe4);
            Lmenu = X11.XKeysymToKeycode(_display, 0xffe9);
            Rmenu = X11.XKeysymToKeycode(_display, 0xffea);
            OemPlus = X11.XKeysymToKeycode(_display, 0x2b);
            OemComma = X11.XKeysymToKeycode(_display, 0x2c);
            OemMinus = X11.XKeysymToKeycode(_display, 0x2d);
            OemPeriod = X11.XKeysymToKeycode(_display, 0x2e);

            //todo verify
            Oem1 = 0x22;
            Oem2 = 0x31;
            Oem3 = 0x23;
            Oem4 = 0x14;
            Oem5 = 0x30;
            Oem6 = 0x15;
            Oem7 = 0x30;
            Oem8 = 0x33;
        }

        public string GetKeyName(uint key)
        {
            var keysym = X11.XkbKeycodeToKeysym(_display, key, 0, 1);
            return X11.XKeysymToString(keysym);
        }

        public uint Back { get; }
        public uint Tab { get; }
        public uint Return { get; }
        public uint Shift { get; }
        public uint Control { get; }
        public uint Menu { get; }
        public uint Pause { get; }
        public uint Capital { get; }
        public uint Escape { get; }
        public uint Space { get; }
        public uint PageUp { get; }
        public uint PageDown { get; }
        public uint End { get; }
        public uint Home { get; }
        public uint Left { get; }
        public uint Up { get; }
        public uint Right { get; }
        public uint Down { get; }
        public uint Insert { get; }
        public uint Delete { get; }
        public uint Help { get; }
        public uint Key0 { get; }
        public uint Key1 { get; }
        public uint Key2 { get; }
        public uint Key3 { get; }
        public uint Key4 { get; }
        public uint Key5 { get; }
        public uint Key6 { get; }
        public uint Key7 { get; }
        public uint Key8 { get; }
        public uint Key9 { get; }
        public uint A { get; }
        public uint B { get; }
        public uint C { get; }
        public uint D { get; }
        public uint E { get; }
        public uint F { get; }
        public uint G { get; }
        public uint H { get; }
        public uint I { get; }
        public uint J { get; }
        public uint K { get; }
        public uint L { get; }
        public uint M { get; }
        public uint N { get; }
        public uint O { get; }
        public uint P { get; }
        public uint Q { get; }
        public uint R { get; }
        public uint S { get; }
        public uint T { get; }
        public uint U { get; }
        public uint V { get; }
        public uint W { get; }
        public uint X { get; }
        public uint Y { get; }
        public uint Z { get; }
        public uint Lwin { get; }
        public uint Rwin { get; }
        public uint Numpad0 { get; }
        public uint Numpad1 { get; }
        public uint Numpad2 { get; }
        public uint Numpad3 { get; }
        public uint Numpad4 { get; }
        public uint Numpad5 { get; }
        public uint Numpad6 { get; }
        public uint Numpad7 { get; }
        public uint Numpad8 { get; }
        public uint Numpad9 { get; }
        public uint Multiply { get; }
        public uint Add { get; }
        public uint Separator { get; }
        public uint Subtract { get; }
        public uint Decimal { get; }
        public uint Divide { get; }
        public uint F1 { get; }
        public uint F2 { get; }
        public uint F3 { get; }
        public uint F4 { get; }
        public uint F5 { get; }
        public uint F6 { get; }
        public uint F7 { get; }
        public uint F8 { get; }
        public uint F9 { get; }
        public uint F10 { get; }
        public uint F11 { get; }
        public uint F12 { get; }
        public uint F13 { get; }
        public uint F14 { get; }
        public uint F15 { get; }
        public uint F16 { get; }
        public uint F17 { get; }
        public uint F18 { get; }
        public uint F19 { get; }
        public uint F20 { get; }
        public uint F21 { get; }
        public uint F22 { get; }
        public uint F23 { get; }
        public uint F24 { get; }
        public uint Lshift { get; }
        public uint Rshift { get; }
        public uint Lcontrol { get; }
        public uint Rcontrol { get; }
        public uint Lmenu { get; }
        public uint Rmenu { get; }
        public uint OemPlus { get; }
        public uint OemComma { get; }
        public uint OemMinus { get; }
        public uint OemPeriod { get; }
        public uint Oem1 { get; }
        public uint Oem2 { get; }
        public uint Oem3 { get; }
        public uint Oem4 { get; }
        public uint Oem5 { get; }
        public uint Oem6 { get; }
        public uint Oem7 { get; }
        public uint Oem8 { get; }
    }
}