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

            //TestBaseCodec();
            TestBase58();
            //TestWallet();
            //TestSpeed();

            //while (true)
            //{
            //    var txout = new byte[32];
            //    using (var rng = RandomNumberGenerator.Create())
            //        rng.GetBytes(txout);

            //    var txs = new List<Transaction>()
            //    {
            //        new Transaction() {
            //            OpCode = OperationCode.Claim,
            //            Timestamp = DateTime.UtcNow,
            //            Input = "00".ToHash(),
            //            Output = txout,
            //            Value = 50
            //        },
            //    };

            //    Block mined = null;
            //    while (!ledger.ProcessBlock(mined))
            //        mined = ledger.CreateBlock(txs);
            //}

            Console.ReadKey();
        }

        private static void TestWallet()
        {


        }

        private static void TestSpeed()
        {
            const uint ITERATIONS = 10_000_000;

            var txout = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(txout);

            var txs = new List<Transaction>()
            {
                new Transaction() {
                    OpCode = OperationCode.Claim,
                    Timestamp = DateTime.UtcNow,
                    Input = "00".ToHash(),
                    Output = txout,
                    Value = 50
                },
            };

            var blk = new Block
            {
                PreviousBlock = "00".ToHash(),
                TxMerkleRoot = txs.OrderBy(tx => tx.Timestamp).Select(tx => tx.GetProof()).GetMerkleRoot(),
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

        private static void TestBase58()
        {
            var b58 = new Base58CheckCodec();
            {
                var addrZero = new byte[20];
                var toBase = b58.Encode(new byte[20]);
                Console.WriteLine($"{toBase} ({toBase.Length})");
                var fromBase = b58.Decode(toBase);
                Console.WriteLine($"{(Hash)fromBase} ({fromBase.Length})");
            }

            {
                var wk = WalletKey.Create();
                var pubKey = wk.PublicKey;
                var pubKeyEncoded = b58.Encode(pubKey);
                var pubKeyDecoded = b58.Decode(pubKeyEncoded);

                //Console.WriteLine($"{(Hash)pubKey} -> {pubKeyEncoded}");
                //Console.WriteLine($"{pubKeyEncoded} -> {(Hash)pubKeyDecoded}");
                //Console.WriteLine($"{(Hash)pubKey} ?= {(Hash)pubKeyDecoded}");

                Console.WriteLine((Hash)pubKey);
                Console.WriteLine(pubKeyEncoded);
                Console.WriteLine((Hash)pubKeyDecoded);
            }


        }

        private static void TestBaseCodec()
        {
            {
                var b58 = new Base58CheckCodec();
                var bytes = new byte[] { 0x0, 0x0, 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF };
                var encoded = b58.Encode(bytes);
                var decoded = b58.Decode(encoded);

                Console.WriteLine((Hash)bytes);
                Console.WriteLine(encoded);
                Console.WriteLine((Hash)decoded);
            }
        }
    }
}