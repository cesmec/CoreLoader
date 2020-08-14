using System;

namespace CoreLoader.OpenGL
{
    internal interface INativeHelper : IDisposable
    {
        IWindowExtensions GetWindowExtensions(INativeWindow window);
        IntPtr GetFunctionPtr(string functionName);
    }
}