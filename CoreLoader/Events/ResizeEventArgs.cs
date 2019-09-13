using System;

namespace CoreLoader.Events
{
    public class ResizeEventArgs : EventArgs
    {
        public int Width { get; }
        public int Height { get; }

        public ResizeEventArgs(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}