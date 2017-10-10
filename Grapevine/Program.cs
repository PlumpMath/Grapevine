using Grapevine.Blockchain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Grapevine
{
    class Program
    {
        static void Main(string[] args)
        {
            var ledger = new Ledger();

            //TestSpeed();
            MineAddresses();

            //while (true)
            //{
            //    var key = WalletKey.Create();

            //    var txs = new List<Transaction>()
            //    {
            //        new Transaction() {
            //            OpCode = OperationCode.Claim,
            //            Timestamp = DateTime.UtcNow,
            //        },
            //    };

            //    txs.First().Outputs.Add(Tuple.Create<string, ulong>(key.Address, 50 * 100_000));

            //    Block mined = null;
            //    while (!ledger.ProcessBlock(mined))
            //        mined = ledger.CreateBlock(txs);
            //}

            Console.ReadKey();
        }

        private static void MineAddresses()
        {
            while (true)
            {
                var key = WalletKey.Create();
                if (key.Address.StartsWith("1Onur"))
                    Console.WriteLine(key.Address);
            }
        }

        private static void TestSpeed()
        {
            const uint ITERATIONS = 10_000_000;

            var key = WalletKey.Create();

            var txs = new List<Transaction>()
            {
                new Transaction() {
                    OpCode = OperationCode.Claim,
                    Timestamp = DateTime.UtcNow,
                },
            };

            txs.First().Outputs.Add(Tuple.Create<string, ulong>(key.Address, 50 * 100_000));

            var blk = new Block
            {
                PreviousBlock = "00".ToHash(),
                TxMerkleRoot = txs
                    .OrderBy(tx => tx.Timestamp)
                    .Select(tx => tx.GetProof())
                    .GetMerkleRoot(),
                Timestamp = DateTime.UtcNow,
                Throttle = 0,
                Transactions = txs.ToList(),
            };

            {
                var sw = Stopwatch.StartNew();
                for (var i = 0; i < ITERATIONS; i++)
                    blk.GetProof();
                sw.Stop();
                Console.WriteLine($"{ITERATIONS} GetProof calls took {sw.Elapsed}");
            }

            //{
            //    var sw = Stopwatch.StartNew();
            //    for (var i = 0; i < 1000000; i++)
            //        blk.GetAltProof();
            //    sw.Stop();
            //    Console.WriteLine($"1000000 GetAltProof {sw.Elapsed}");
            //}
        }
    }
}