using System;
using System.Runtime.InteropServices;
using CoreLoader.Unix.Native;

namespace CoreLoader.OpenGL.Unix
{
    internal class OpenGl
    {
        private const string LibName = "libGL";

        [DllImport(LibName, ExactSpelling = true, EntryPoint = "glXGetProcAddress")]
        public static extern IntPtr GlXGetProcAddress(string functionName);
        [DllImport(LibName, ExactSpelling = true, EntryPoint = "glXGetProcAddressARB")]
        public static extern IntPtr GlXGetProcAddressArb(string functionName);
        [DllImport(LibName, ExactSpelling = true, EntryPoint = "glXChooseVisual")]
        public static extern IntPtr GlXChooseVisual(IntPtr display, int screen, int[] attributes);
        [DllImport(LibName, ExactSpelling = true, EntryPoint = "glXCreateContext")]
        public static extern IntPtr GlXCreateContext(IntPtr display, ref X11.XVisualInfo visualInfo, IntPtr shareList, bool direct);
        [DllImport(LibName, ExactSpelling = true, EntryPoint = "glXMakeCurrent")]
        public static extern bool GlXMakeCurrent(IntPtr display, ulong drawable, IntPtr context);
        [DllImport(LibName, ExactSpelling = true, EntryPoint = "glXDestroyContext")]
        public static extern void GlXDestroyContext(IntPtr display, IntPtr context);
        [DllImport(LibName, ExactSpelling = true, EntryPoint = "glXSwapBuffers")]
        public static extern void GlXSwapBuffers(IntPtr displayPtr, uint window);
    }
}