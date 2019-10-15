using System;

namespace CoreLoader.Events
{
    public class MouseMoveEventArgs : EventArgs
    {
        public int X { get; }
        public int Y { get; }

        public MouseMoveEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}