using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Grapevine.Blockchain
{
    public class Block : IEnumerable<Transaction>, IHashIdentifier, IVerifiable
    {
        public Hash Identifier => Header.Identifier;

        // The header of a block
        public BlockHeader Header { get; set; }
        // The height of a block
        public UInt64 BlockHeight { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public IEnumerator<Transaction> GetEnumerator() => Transactions.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Transaction this[string digest] => Transactions.FirstOrDefault(tx => tx.GetProof().ToString() == digest);
        public Transaction this[int idx] => Transactions.OrderBy(tx => tx.Timestamp).ElementAtOrDefault(idx);

        public bool IsValid(Ledger blockchain)
        {
            // A block is valid if all transactions in the block is valid
            if (!Transactions.All(tx => tx.IsValid(blockchain)))
                return false;
            throw new BlockIntegrityException();
        }
    }
}
