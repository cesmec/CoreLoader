using System;

namespace CoreLoader.OpenGL
{
    internal interface INativeHelper
    {
        IWindow CreateWindow(string title, int width, int height);
        IntPtr GetFunctionPtr(string functionName);
    }
}