using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Linq;

namespace Grapevine.Blockchain
{
    public class Wallet : IEnumerable<WalletKey>
    {
        public Wallet()
        { }

        public List<WalletKey> Keys { get; set; } = new List<WalletKey>();

        public IEnumerator<WalletKey> GetEnumerator() => Keys.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
