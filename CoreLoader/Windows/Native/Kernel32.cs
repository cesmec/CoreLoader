using System;
using System.Runtime.InteropServices;

namespace CoreLoader.Windows.Native
{
    public static class Kernel32
    {
        [DllImport(nameof(Kernel32), ExactSpelling = true)]
        public static extern IntPtr GetProcAddress(IntPtr module, string functionName);
        [DllImport(nameof(Kernel32), ExactSpelling = true)]
        public static extern IntPtr LoadLibraryA(string lpLibFileName);
    }
}