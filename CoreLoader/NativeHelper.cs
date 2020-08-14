using System;
using CoreLoader.Unix;
using CoreLoader.Windows;

namespace CoreLoader
{
    internal static class NativeHelper
    {
        internal static INativeWindow CreateWindow(string title, int width, int height)
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    if (!Environment.Is64BitOperatingSystem)
                        throw new PlatformNotSupportedException($"Only 64-bit unix systems are supported");
                    return new X11Window(title, width, height);

                case PlatformID.Win32NT:
                    return new Win32Window(title, width, height);

                default:
                    throw new PlatformNotSupportedException();
            }
        }
    }
}