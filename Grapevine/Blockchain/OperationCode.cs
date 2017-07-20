using System;

namespace Grapevine.Blockchain
{
    // aka opcode
    public enum OperationCode : Byte
    {
        Claim = 0,
        Fee,
        Transfer,
    }
}
