using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace Grapevine.Blockchain
{
    public class Ledger : IEnumerable<Block>, IVerifiable
    {
        private static TimeSpan s_RetargetTimespan = TimeSpan.FromMinutes(10);
        private static uint s_RetargetBlockspan = 512;

        public Ledger()
        {

        }

        public int Difficulty { get; private set; } = 15;
        public BigInteger Target => BigInteger.Pow(2, 256 - Difficulty);

        public Block CreateBlock(IEnumerable<Transaction> txs)
        {
            var blk = new Block
            {
                PreviousBlock = Head?.Identifier ?? "00".ToHash(),
                TxMerkleRoot = txs.OrderBy(tx => tx.Timestamp).Select(tx => tx.GetProof()).GetMerkleRoot(),
                Timestamp = DateTime.UtcNow,
                Throttle = (uint)Difficulty,
                Transactions = txs.ToList(),
            };

            var startTime = DateTime.UtcNow;
            do
            {
                if (blk.Identifier.ToBigInteger() <= Target)
                {
                    var endTime = DateTime.UtcNow;

                    Console.WriteLine($"Success with nonce {blk.Nonce}");
                    Console.WriteLine($"Hash is {blk.Identifier}");

                    var elapsedTime = endTime - startTime;
                    Console.WriteLine($"Elapsed time: {elapsedTime} - Hashing power: {(uint)(blk.Nonce / elapsedTime.TotalSeconds)} hps");

                    Console.WriteLine();

                    return blk;
                }
            } while (blk.Nonce++ < uint.MaxValue);

            return null;
        }

        public bool ProcessBlock(Block blk)
        {
            if (blk == null)
                return false;

            //if (!blk.IsValid(this))
            //    return false;

            if (Head == null)
                blk.BlockHeight = 0;
            else
            {
                if (!blk.PreviousBlock.Equals(Head.Identifier))
                    return false;
                blk.BlockHeight = Head.BlockHeight + 1;
            }

            //if (blk.Throttle != Difficulty)
            //    return false;

            if (blk.Identifier.ToBigInteger() > Target)
                return false;

            Blocks.Add(blk);
            Head = blk;

            if (blk.BlockHeight > 0 && (blk.BlockHeight % s_RetargetBlockspan) == 0)
            {
                var prevRetargetBlock = FindBlockByDepth(s_RetargetBlockspan);
                var retargetPeriodSpan = blk.Timestamp - prevRetargetBlock.Timestamp;
                Console.WriteLine($"Difficulty {BigInteger.Pow(2, Difficulty)} ({Difficulty} bits)");
                Console.WriteLine($"Target is {Target.ToString("x32")}");
                Console.WriteLine($"Blockspan {s_RetargetBlockspan} time {retargetPeriodSpan} target {s_RetargetTimespan}");
                if ((retargetPeriodSpan - s_RetargetTimespan).Duration().TotalSeconds > 60)
                {
                    Console.WriteLine("Need to retarget as it differs by more than 60 seconds");
                    Difficulty = Math.Min(Math.Max(0, Difficulty + (s_RetargetTimespan > retargetPeriodSpan ? 1 : -1)), 256);
                    Console.WriteLine($"New difficulty {BigInteger.Pow(2, Difficulty)} ({Difficulty} bits)");
                    Console.WriteLine($"Target is {Target.ToString("x32")}");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"{blk.Identifier} accepted at height {blk.BlockHeight}");

            return true;
        }



        public Block FindBlockByDepth(ulong depth) =>
            FindBlockByHeight(Head.BlockHeight - depth);
        /*
        public Block FindBlockByDepth(ulong depth)
        {
            var blk = Head;
            for (var i = 0ul; i < depth; i++)
                blk = this[blk.Header.PreviousBlock];
            return blk;
        }
        */

        public Block FindBlockByHeight(ulong height) =>
            Blocks.FirstOrDefault(b => b.BlockHeight == height);
        /*
        public Block FindBlockByHeight(ulong height)
        {
            if (Head.BlockHeight < height) return null;

            var blk = Head;
            do
            {
                blk = this[blk.Header.PreviousBlock];
            } while (blk.BlockHeight > height);

            return blk;
        }
        */

        public Block Head { get; private set; }

        public List<Block> Blocks { get; private set; }
            = new List<Block>();

        public IEnumerator<Block> GetEnumerator() =>
            Blocks.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public Block this[Hash hash] =>
            Blocks.FirstOrDefault(b => b.Identifier == hash);
        public Block this[string digest] =>
            this[digest.ToHash()];
        public Block this[ulong height] =>
            Blocks.FirstOrDefault(b => b.BlockHeight == height);

        public Block GetPrevious(Block block) =>
            this[block.PreviousBlock];

        public bool IsValid(Ledger blockchain) =>
            Blocks.All(b => b.IsValid(this));
    }
}
