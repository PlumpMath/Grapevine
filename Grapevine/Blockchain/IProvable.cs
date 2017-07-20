using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Grapevine.Blockchain
{
    interface IProvable
    {
        Hash GetProof();
    }
}
