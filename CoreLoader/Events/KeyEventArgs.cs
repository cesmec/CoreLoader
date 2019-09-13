using System;

namespace CoreLoader.Events
{
    public class KeyEventArgs : EventArgs
    {
        public uint Key { get; }

        public KeyEventArgs(uint key)
        {
            Key = key;
        }
    }
}