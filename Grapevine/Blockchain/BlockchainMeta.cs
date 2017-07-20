using System;
using System.Numerics;

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
