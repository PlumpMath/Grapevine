using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Grapevine.Blockchain
{
    public class Block : IEnumerable<Transaction>, IVerifiable, IProvable, IHashIdentifier
    {
        public Block()
        { }

        public Block(IEnumerable<Transaction> txs)
        {
            Transactions = txs.ToList();
        }

        public Hash Identifier => GetProof();

        private UInt32 BlockVersion { get; set; } = 0;
        public Hash PreviousBlock { get; set; }
        public Hash TxMerkleRoot { get; set; }
        public DateTime Timestamp { get; set; }
        public UInt32 Throttle { get; set; }
        public UInt32 Nonce { get; set; }

        public Hash GetProof()
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write((UInt32)BlockVersion);
                bw.Write((byte[])PreviousBlock);
                bw.Write((byte[])TxMerkleRoot);
                bw.Write((UInt32)Timestamp.ToEpoch());
                bw.Write((UInt32)Throttle);
                bw.Write((UInt32)Nonce);

                return HashUtil.ComputeSHA256(HashUtil.ComputeSHA256(ms.ToArray()));
            }
        }

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
