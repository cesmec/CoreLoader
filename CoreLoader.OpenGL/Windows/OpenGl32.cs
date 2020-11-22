using System;
using System.Runtime.InteropServices;

namespace CoreLoader.OpenGL.Windows
{
    internal static class OpenGl32
    {
        [DllImport(nameof(OpenGl32), ExactSpelling = true, EntryPoint = "wglGetProcAddress", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern IntPtr WglGetProcAddress(string functionName);
        [DllImport(nameof(OpenGl32), ExactSpelling = true, EntryPoint = "wglCreateContext")]
        public static extern IntPtr WglCreateContext(IntPtr deviceContext);
        [DllImport(nameof(OpenGl32), ExactSpelling = true, EntryPoint = "wglMakeCurrent")]
        public static extern bool WglMakeCurrent(IntPtr deviceContext, IntPtr context);
        [DllImport(nameof(OpenGl32), ExactSpelling = true, EntryPoint = "wglDeleteContext")]
        public static extern bool WglDeleteContext(IntPtr context);

        public unsafe delegate IntPtr WglCreateContextAttribsArbProc(IntPtr hDC, IntPtr hshareContext, int* attribList);

        public static WglCreateContextAttribsArbProc GetWglCreateContextAttribsArbProc()
        {
            var address = WglGetProcAddress("wglCreateContextAttribsARB");
            return Marshal.GetDelegateForFunctionPointer<WglCreateContextAttribsArbProc>(address);
        }
    }
}