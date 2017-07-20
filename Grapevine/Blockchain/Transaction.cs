using System;
using System.IO;

namespace Grapevine.Blockchain
{
    public class Transaction : IProvable, IHashIdentifier, IVerifiable
    {
        public Hash Identifier => GetProof();

        private UInt32 TxVersion { get; set; } = 0;
        public DateTime Timestamp { get; set; }
        public OperationCode OpCode { get; set; }
        public Hash Input { get; set; }
        public Hash Output { get; set; }
        public UInt64 Value { get; set; }

        public Hash GetProof()
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write((UInt32)TxVersion);
                bw.Write((UInt32)Timestamp.ToEpoch());
                bw.Write((byte)OpCode);
                bw.Write((byte[])Input);
                bw.Write((byte[])Output);
                bw.Write((UInt64)Value);

                return Hash.HashTwice(ms.ToArray());
            }
        }

        public bool IsValid(Ledger blockchain)
        {
            throw new TransactionIntegrityException();
        }
    }
}
