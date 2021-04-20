using System;

namespace CoreLoader.Events
{
    public class ScrollEventArgs : EventArgs
    {
        public ScrollDirection Direction { get; }
        public Point MousePosition { get; }
        public int Value { get; }

        public ScrollEventArgs(ScrollDirection direction, Point mousePosition, int value)
        {
            Direction = direction;
            MousePosition = mousePosition;
            Value = value;
        }
    }
}