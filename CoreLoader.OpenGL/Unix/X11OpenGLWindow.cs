using System;
using System.Runtime.InteropServices;
using CoreLoader.Unix;
using CoreLoader.Unix.Native;

namespace CoreLoader.OpenGL.Unix
{
    internal sealed class X11OpenGLWindow : X11Window
    {
        private readonly IntPtr _contextPtr;
        private X11.XVisualInfo _visualInfo;

        public X11OpenGLWindow(string title, int width, int height) : base(title, width, height)
        {
            _contextPtr = OpenGl.GlXCreateContext(DisplayPtr, ref _visualInfo, IntPtr.Zero, true);
            OpenGl.GlXMakeCurrent(DisplayPtr, WindowId, _contextPtr);
        }

        public override void SwapBuffers()
        {
            OpenGl.GlXSwapBuffers(DisplayPtr, WindowId);
        }

        protected override X11.XVisualInfo GetVisualInfo(X11.XDisplay display)
        {
            var attributes = new[] { 4 /*GLX_RGBA*/, 12 /*GLX_DEPTH_SIZE*/, 24, 5 /*GLX_DOUBLEBUFFER*/ };
            var visualInfoPtr = OpenGl.GlXChooseVisual(DisplayPtr, display.default_screen, attributes);
            _visualInfo = Marshal.PtrToStructure<X11.XVisualInfo>(visualInfoPtr);

            return _visualInfo;
        }

        protected override void Cleanup()
        {
            OpenGl.GlXMakeCurrent(DisplayPtr, 0, IntPtr.Zero);
            OpenGl.GlXDestroyContext(DisplayPtr, _contextPtr);
        }
    }
}