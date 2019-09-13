using System;
using System.Runtime.InteropServices;

namespace CoreLoader.Windows.Native
{
    public static class Gdi32
    {
        public struct PixelFormatDescriptor
        {
            public ushort nSize;
            public ushort nVersion;
            public uint dwFlags;
            public byte iPixelType;
            public byte cColorBits;
            public byte cRedBits;
            public byte cRedShift;
            public byte cGreenBits;
            public byte cGreenShift;
            public byte cBlueBits;
            public byte cBlueShift;
            public byte cAlphaBits;
            public byte cAlphaShift;
            public byte cAccumBits;
            public byte cAccumRedBits;
            public byte cAccumGreenBits;
            public byte cAccumBlueBits;
            public byte cAccumAlphaBits;
            public byte cDepthBits;
            public byte cStencilBits;
            public byte cAuxBuffers;
            public byte iLayerType;
            public byte bReserved;
            public uint dwLayerMask;
            public uint dwVisibleMask;
            public uint dwDamageMask;
        }

        [DllImport(nameof(Gdi32), ExactSpelling = true)]
        public static extern int ChoosePixelFormat(IntPtr deviceContext, ref PixelFormatDescriptor pfd);
        [DllImport(nameof(Gdi32), ExactSpelling = true)]
        public static extern int SetPixelFormat(IntPtr deviceContext, int pixelFormat, ref PixelFormatDescriptor pfd);
        [DllImport(nameof(Gdi32), ExactSpelling = true)]
        public static extern bool SwapBuffers(IntPtr deviceContext);
    }
}