using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grapevine
{
    public class Hash
    {
        public Hash(byte[] digest) => Digest = digest;
        public byte[] Digest { get; private set; }

        public static implicit operator Hash(byte[] digest) => new Hash(digest);
        public static explicit operator byte[] (Hash hash) => hash.Digest;
        public override string ToString() => string.Join("", Digest.Select(b => b.ToString("x2")));
    }
}
