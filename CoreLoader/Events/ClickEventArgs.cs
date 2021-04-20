using System;

namespace CoreLoader.Events
{
    public class ClickEventArgs : EventArgs
    {
        public enum MouseButton : uint
        {
            Left = 0,
            Right = 1,
            Middle = 2,
            Extra1 = 3,
            Extra2 = 4
        }

        public ClickEventState State { get; }
        public Point MousePosition { get; }
        public MouseButton Button { get; }

        public ClickEventArgs(ClickEventState state, Point mousePosition, MouseButton button)
        {
            State = state;
            MousePosition = mousePosition;
            Button = button;
        }

        public static ClickEventArgs Down(Point mousePosition, MouseButton button)
        {
            return new ClickEventArgs(ClickEventState.Down, mousePosition, button);
        }

        public static ClickEventArgs Up(Point mousePosition, MouseButton button)
        {
            return new ClickEventArgs(ClickEventState.Up, mousePosition, button);
        }
    }
}