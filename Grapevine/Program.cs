﻿using Grapevine.Blockchain;
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

            TestWallet();
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
            var wallet = new Wallet();
            var wk = WalletKey.Create();
            wallet.Keys.Add(wk);

            var signedMessage = "Wallet Test 2017";
            var unsignedMessage = "Tallet West 1720";

            Console.WriteLine($"Signing message '{signedMessage}'...");
            var signature = wk.DSA.SignData(Encoding.UTF8.GetBytes(signedMessage), HashAlgorithmName.SHA256);
            Console.WriteLine($"Signature: {(Hash)signature}");
            {
                Console.WriteLine($"Signature matches '{signedMessage}': {wk.DSA.VerifyData(Encoding.UTF8.GetBytes(signedMessage), signature, HashAlgorithmName.SHA256)}");
            }
            {
                Console.WriteLine($"Signature matches '{unsignedMessage}': {wk.DSA.VerifyData(Encoding.UTF8.GetBytes(unsignedMessage), signature, HashAlgorithmName.SHA256)}");
            }

            Console.WriteLine("Exporting->importing key");
            var wk1 = new WalletKey(wallet.FirstOrDefault().DSA.ExportExplicitParameters(true));
            {
                Console.WriteLine($"Signature matches '{signedMessage}': {wk1.DSA.VerifyData(Encoding.UTF8.GetBytes(signedMessage), signature, HashAlgorithmName.SHA256)}");
            }
            {
                Console.WriteLine($"Signature matches '{unsignedMessage}': {wk1.DSA.VerifyData(Encoding.UTF8.GetBytes(unsignedMessage), signature, HashAlgorithmName.SHA256)}");
            }

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
    }
}