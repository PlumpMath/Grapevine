using Grapevine.Blockchain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

namespace Grapevine
{
    class Program
    {
        static void Main(string[] args)
        {
            var txs = new List<Transaction>()
            {
                new Transaction() { OpCode = OperationCode.Claim, Timestamp = DateTime.UtcNow, Input = "00".ToHash(), Output = "01".ToHash(), Value = 50 },
                new Transaction() { OpCode = OperationCode.Transfer, Timestamp = DateTime.UtcNow, Input = "01".ToHash(), Output = "02".ToHash(), Value = 2 },
                new Transaction() { OpCode = OperationCode.Transfer, Timestamp = DateTime.UtcNow, Input = "02".ToHash(), Output = "03".ToHash(), Value = 4 },
                new Transaction() { OpCode = OperationCode.Transfer, Timestamp = DateTime.UtcNow, Input = "03".ToHash(), Output = "04".ToHash(), Value = 6 },
                new Transaction() { OpCode = OperationCode.Transfer, Timestamp = DateTime.UtcNow, Input = "04".ToHash(), Output = "05".ToHash(), Value = 8 },
                new Transaction() { OpCode = OperationCode.Transfer, Timestamp = DateTime.UtcNow, Input = "05".ToHash(), Output = "06".ToHash(), Value = 10 },
                new Transaction() { OpCode = OperationCode.Transfer, Timestamp = DateTime.UtcNow, Input = "06".ToHash(), Output = "07".ToHash(), Value = 12 },
                new Transaction() { OpCode = OperationCode.Transfer, Timestamp = DateTime.UtcNow, Input = "07".ToHash(), Output = "08".ToHash(), Value = 14 },
                new Transaction() { OpCode = OperationCode.Transfer, Timestamp = DateTime.UtcNow, Input = "09".ToHash(), Output = "0A".ToHash(), Value = 16 },
                new Transaction() { OpCode = OperationCode.Transfer, Timestamp = DateTime.UtcNow, Input = "0A".ToHash(), Output = "0B".ToHash(), Value = 18 },
            };

            var hdr = new BlockHeader()
            {
                PreviousBlock = new string('0', 32 * 2).ToHash(),
                TxMerkleRoot = txs.OrderBy(tx => tx.Timestamp).Select(tx => tx.GetProof()).GetMerkleRoot(),
                Timestamp = DateTime.UtcNow,
            };

            var genesis = new Block()
            {
                Header = hdr,
                Transactions = txs,
            };

            var sw = Stopwatch.StartNew();
            do
            {
                genesis.Header.Nonce++;

                if (genesis.Identifier.ToString().StartsWith("000000"))
                {
                    Console.WriteLine($"{genesis.Identifier} after {sw.Elapsed}");
                }
            } while (genesis.Header.Nonce < UInt32.MaxValue);

            Console.ReadKey();
        }
    }
}