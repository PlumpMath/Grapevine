using System;
using System.Collections.Generic;
using System.Text;

namespace Grapevine.Blockchain
{
    interface IVerifiable
    {
        bool IsValid(Ledger blockchain);
    }
}
