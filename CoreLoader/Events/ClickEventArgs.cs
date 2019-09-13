using System;

namespace CoreLoader.Events
{
    public class ClickEventArgs : EventArgs
    {
        public const uint Left = 0;
        public const uint Right = 1;
        public const uint Middle = 2;

        public ClickEventState State { get; }
        public uint Button { get; }

        public ClickEventArgs(ClickEventState state, uint button)
        {
            State = state;
            Button = button;
        }

        public static ClickEventArgs Down(uint button)
        {
            return new ClickEventArgs(ClickEventState.Down, button);
        }

        public static ClickEventArgs Up(uint button)
        {
            return new ClickEventArgs(ClickEventState.Up, button);
        }
    }
}