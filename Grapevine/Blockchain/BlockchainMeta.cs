using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Grapevine.Blockchain
{
    public class BlockchainMeta
    {
        public Hash Head { get; set; }
        public UInt64 Height { get; set; } // TODO: Determine type
        public BigInteger Target { get; set; }
        public BigInteger Difficulty { get; set; }
    }
}
