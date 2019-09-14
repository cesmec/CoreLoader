using System;

namespace CoreLoader.Events
{
    public class KeyEventArgs : EventArgs
    {
        public uint Key { get; }
        //todo modifier keys

        public KeyEventArgs(uint key)
        {
            Key = key;
        }
    }
}