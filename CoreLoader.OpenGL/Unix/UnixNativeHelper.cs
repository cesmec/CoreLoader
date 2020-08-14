using System;

namespace CoreLoader.OpenGL.Unix
{
    internal sealed class UnixNativeHelper : INativeHelper
    {
        public IWindowExtensions GetWindowExtensions(INativeWindow window)
        {
            return new X11OpenGLWindowExtensions(window);
        }

        public IntPtr GetFunctionPtr(string functionName)
        {
            var address = OpenGl.GlXGetProcAddress(functionName);
            if (address == IntPtr.Zero)
            {
                address = OpenGl.GlXGetProcAddressArb(functionName);
            }
            return address;
        }

        public void Dispose()
        { }
    }
}