using System;
using System.Collections.Generic;
using System.Text;

namespace Grapevine.Blockchain
{
    public class Block
    {
        // The hash of a block
        public Hash BlockHash { get; set; }
        // The header of a block
        public BlockHeader Header { get; set; }
        // The height of a block
        public UInt64 BlockHeight { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        // Block verification
        public bool IsValid =>
            Header.CompactTarget <= Header.CompactTarget // todo: proper
            && Header.GenerateProof() == BlockHash;
    }
}
