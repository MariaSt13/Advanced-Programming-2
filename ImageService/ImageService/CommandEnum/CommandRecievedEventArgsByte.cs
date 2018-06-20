using System;

namespace Infrastructure
{
    public class CommandRecievedEventArgsByte : EventArgs
    {
        public byte[] Args { get; set; }

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="args"></param>
        public CommandRecievedEventArgsByte(byte[] args)
        {
            Args = args;
        }
    }
}