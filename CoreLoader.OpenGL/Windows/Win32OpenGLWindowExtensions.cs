using System;
using CoreLoader.Windows.Native;

namespace CoreLoader.OpenGL.Windows
{
    internal sealed class Win32OpenGLWindowExtensions : IWindowExtensions
    {
        private readonly INativeWindow _window;
        private IntPtr _deviceContext;
        private IntPtr _openGlContext;

        public Win32OpenGLWindowExtensions(INativeWindow window)
        {
            _window = window;
        }

        public void SwapBuffers()
        {
            Gdi32.SwapBuffers(_deviceContext);
        }

        public void Cleanup()
        {
            OpenGl32.WglDeleteContext(_openGlContext);
        }

        public void OnShow()
        {
            MakeContextCurrent();

            WindowExtensions.LoadFunctions<GlNative>();
        }

        private void MakeContextCurrent()
        {
            unsafe
            {
                var pfdSize = sizeof(Gdi32.PixelFormatDescriptor);
                var pfd = new Gdi32.PixelFormatDescriptor
                {
                    nSize = (ushort)pfdSize,
                    nVersion = 1,
                    dwFlags = 0x4 /*PFD_DRAW_TO_WINDOW*/ | 0x20 /*PFD_SUPPORT_OPENGL*/ | 0x1 /*PFD_DOUBLEBUFFER*/,
                    iPixelType = 0 /*PFD_TYPE_RGBA*/,
                    cColorBits = 32,
                    cDepthBits = 32,
                    cStencilBits = 8,
                    cAuxBuffers = 0,
                    iLayerType = 0 /*PFD_MAIN_PLANE*/
                };
                _deviceContext = User32.GetDC(_window.NativeHandle);

                var pixelFormat = Gdi32.ChoosePixelFormat(_deviceContext, ref pfd);
                Gdi32.SetPixelFormat(_deviceContext, pixelFormat, ref pfd);

                var tempContext = OpenGl32.WglCreateContext(_deviceContext);
                OpenGl32.WglMakeCurrent(_deviceContext, tempContext);

                var wglCreateContextAttribsArb = OpenGl32.GetWglCreateContextAttribsArbProc();

                var attribs = new[] { /*WGL_CONTEXT_PROFILE_MASK_ARB*/ 0x9126, /*WGL_CONTEXT_CORE_PROFILE_BIT_ARB*/ 0x00000001, 0 };
                fixed (int* attribPtr = attribs)
                    _openGlContext = wglCreateContextAttribsArb(_deviceContext, IntPtr.Zero, attribPtr);

                OpenGl32.WglMakeCurrent(_deviceContext, _openGlContext);
                OpenGl32.WglDeleteContext(tempContext);
            }
        }
    }
}