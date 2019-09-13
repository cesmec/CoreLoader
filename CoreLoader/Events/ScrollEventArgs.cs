using System;

namespace CoreLoader.Events
{
    public class ScrollEventArgs : EventArgs
    {
        public ScrollDirection Direction { get; }
        public int Value { get; }

        public ScrollEventArgs(ScrollDirection direction, int value)
        {
            Direction = direction;
            Value = value;
        }
    }
}