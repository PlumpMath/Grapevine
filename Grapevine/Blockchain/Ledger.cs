using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grapevine.Blockchain
{
    public class Ledger : IEnumerable<Block>, IVerifiable
    {
        public Ledger()
        {

        }

        public List<Block> Blocks { get; private set; } = new List<Block>();

        public IEnumerator<Block> GetEnumerator() => Blocks.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Block this[Hash hash] => Blocks.FirstOrDefault(b => b.Header.Identifier == hash);
        public Block this[string digest] => this[digest.ToHash()];
        public Block this[ulong height] => Blocks.FirstOrDefault(b => b.BlockHeight == height);

        public Block GetPrevious(Block block) => this[block.Header.PreviousBlock];

        public bool IsValid(Ledger blockchain) => Blocks.All(b => b.IsValid(this));
    }
}
