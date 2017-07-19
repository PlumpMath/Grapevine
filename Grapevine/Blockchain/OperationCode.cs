using System;
using System.Collections.Generic;
using System.Text;

namespace Grapevine.Blockchain
{
    // aka opcode
    public enum OperationCode : byte
    {
        Claim = 0,
        Fee,
        Transfer,
    }
}
