using System;

namespace LoriaNET
{
    public class ListenerEventArgs : EventArgs
    {
        public Command Command { get; set; }

        public ListenerEventArgs(Command command)
        {
            Command = command;
        }
    }
}
