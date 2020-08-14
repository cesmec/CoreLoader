using System;
using System.Runtime.InteropServices;

namespace CoreLoader.OpenGL.Windows
{
    internal sealed class WindowsNativeHelper : INativeHelper
    {
        private readonly IntPtr _openGlLibrary;

        public WindowsNativeHelper()
        {
            _openGlLibrary = NativeLibrary.Load("opengl32.dll");
        }

        public IWindowExtensions GetWindowExtensions(INativeWindow window)
        {
            return new Win32OpenGLWindowExtensions(window);
        }

        public IntPtr GetFunctionPtr(string functionName)
        {
            var address = OpenGl32.WglGetProcAddress(functionName);
            if (address == IntPtr.Zero)
            {
                NativeLibrary.TryGetExport(_openGlLibrary, functionName, out address);
            }
            return address;
        }

        public void Dispose()
        {
            NativeLibrary.Free(_openGlLibrary);
        }
    }
}