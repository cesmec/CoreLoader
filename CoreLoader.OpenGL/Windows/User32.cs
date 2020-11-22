using System;
using System.Runtime.InteropServices;

namespace CoreLoader.OpenGL.Windows
{
    internal static class User32
    {
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);
    }
}