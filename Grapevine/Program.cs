using Grapevine.Blockchain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;

namespace Grapevine
{
    class Program
    {
        static void Main(string[] args)
        {
            var ledger = new Ledger();

            while (true)
            {
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

                Block mined = null;
                while (!ledger.ProcessBlock(mined))
                    mined = ledger.CreateBlock(txs);
            }

            Console.ReadKey();
        }
    }
}