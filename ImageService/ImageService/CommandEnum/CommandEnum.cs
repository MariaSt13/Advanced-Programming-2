using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandEnum
{
    /// <summary>
    /// type of commands
    /// </summary>
    public enum CommandEnum : int
    {
        NewFileCommand,
        GetConfigCommand,
        LogCommand,
        CloseCommand
    }
}
