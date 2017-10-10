using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Grapevine.Blockchain
{
    public class Transaction : IProvable, IHashIdentifier, IVerifiable
    {
        public Hash Identifier => GetProof();

        private UInt32 TxVersion { get; set; } = 0;
        public DateTime Timestamp { get; set; }
        public OperationCode OpCode { get; set; }
        public List<Tuple<string, UInt64>> Inputs { get; }
            = new List<Tuple<string, ulong>>();
        public List<Tuple<string, UInt64>> Outputs { get; }
            = new List<Tuple<string, ulong>>();

        public Hash GetProof()
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write((UInt32)TxVersion);
                bw.Write((UInt32)Timestamp.ToEpoch());
                bw.Write((byte)OpCode);

                bw.Write((UInt32)Inputs.Count);
                foreach (var (address, amount) in Inputs)
                {
                    bw.Write((string)address);
                    bw.Write((UInt64)amount);
                }

                bw.Write((UInt32)Outputs.Count);
                foreach (var (address, amount) in Outputs)
                {
                    bw.Write((string)address);
                    bw.Write((UInt64)amount);
                }

                return HashUtil.Compute(HashUtil.Compute(ms.ToArray()));
            }
        }

        public bool IsValid(Ledger blockchain)
        {
            throw new TransactionIntegrityException();
        }
    }
}
