using System;

namespace CoreLoader.OpenGL.Unix
{
    internal sealed class UnixNativeHelper : INativeHelper
    {
        public IWindow CreateWindow(string title, int width, int height)
        {
            return new X11OpenGLWindow(title, width, height);
        }

        public void Init()
        { }

        public IntPtr GetFunctionPtr(string functionName)
        {
            var address = OpenGl.GlXGetProcAddress(functionName);
            if (address == IntPtr.Zero)
            {
                address = OpenGl.GlXGetProcAddressArb(functionName);
            }
            return address;
        }
    }
}