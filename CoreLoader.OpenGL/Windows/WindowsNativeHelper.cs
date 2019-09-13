﻿using System;
using CoreLoader.Windows.Native;

namespace CoreLoader.OpenGL.Windows
{
    internal sealed class WindowsNativeHelper : INativeHelper
    {
        private IntPtr _openGlLibrary;

        public IWindow CreateWindow(string title, int width, int height)
        {
            return new Win32OpenGLWindow(title, width, height);
        }

        public void Init()
        {
            _openGlLibrary = Kernel32.LoadLibraryA("opengl32.dll");
        }

        public IntPtr GetFunctionPtr(string functionName)
        {
            var address = OpenGl32.WglGetProcAddress(functionName);
            if (address == IntPtr.Zero)
            {
                address = Kernel32.GetProcAddress(_openGlLibrary, functionName);
            }
            return address;
        }
    }
}