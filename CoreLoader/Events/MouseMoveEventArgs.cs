using System;

namespace CoreLoader.Events
{
    public class MouseMoveEventArgs : EventArgs
    {
        public float X { get; }
        public float Y { get; }

        public MouseMoveEventArgs(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}