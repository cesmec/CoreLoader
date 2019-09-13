using System;

namespace CoreLoader
{
    internal interface INativeHelper
    {
        IWindow CreateWindow(string title, int width, int height);
        void Init();
        IntPtr GetFunctionPtr(string functionName);
    }
}