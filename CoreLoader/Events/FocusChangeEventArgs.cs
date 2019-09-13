using System;

namespace CoreLoader.Events
{
    public class FocusChangeEventArgs : EventArgs
    {
        public bool HasFocus { get; }

        public FocusChangeEventArgs(bool hasFocus)
        {
            HasFocus = hasFocus;
        }
    }
}