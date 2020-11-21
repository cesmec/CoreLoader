using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CoreLoader.Unix;
using CoreLoader.Unix.Native;

namespace CoreLoader.OpenGL.Unix
{
    internal sealed class X11OpenGLWindowExtensions : IX11WindowExtensions, IOpenGLWindowExtensions
    {
        private readonly INativeWindow _window;
        private IntPtr _contextPtr;
        private X11.XVisualInfo _visualInfo;
        private X11.XDisplay _display;

        public X11OpenGLWindowExtensions(INativeWindow window)
        {
            _window = window;
        }

        public X11.XVisualInfo GetVisualInfo(X11.XDisplay display)
        {
            var attributes = new[] { 4 /*GLX_RGBA*/, 12 /*GLX_DEPTH_SIZE*/, 24, 5 /*GLX_DOUBLEBUFFER*/ };
            var visualInfoPtr = OpenGl.GlXChooseVisual(_window.NativeHandle, display.default_screen, attributes);
            _visualInfo = Marshal.PtrToStructure<X11.XVisualInfo>(visualInfoPtr);
            _display = display;

            return _visualInfo;
        }

        public void OnShow()
        {
            _contextPtr = OpenGl.GlXCreateContext(_window.NativeHandle, ref _visualInfo, IntPtr.Zero, true);
            OpenGl.GlXMakeCurrent(_window.NativeHandle, _window.WindowId, _contextPtr);

            WindowExtensions.LoadDefaultOpenGLFunctions();
        }

        public void SwapBuffers()
        {
            OpenGl.GlXSwapBuffers(_window.NativeHandle, _window.WindowId);
        }

        public void Cleanup()
        {
            OpenGl.GlXMakeCurrent(_window.NativeHandle, 0, IntPtr.Zero);
            OpenGl.GlXDestroyContext(_window.NativeHandle, _contextPtr);
            WindowExtensions.Cleanup();
        }

        public IReadOnlyList<string> GetPlatformExtensions()
        {
            var extensions = OpenGl.GlXQueryExtensionsString(_window.NativeHandle, _display.default_screen);
            return extensions.Split(' ');
        }
    }
}