using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Grapevine.Blockchain
{
    public class Hash
    {
        public Hash(byte[] digest) => Digest = digest;
        public byte[] Digest { get; private set; }

        public static implicit operator Hash(byte[] digest) => new Hash(digest);
        public static explicit operator byte[] (Hash hash) => hash.Digest;
        public override string ToString() => string.Join("", Digest.Select(b => b.ToString("x2")));

        private static HashAlgorithm _sha256 = SHA256.Create();
        public static Hash HashTwice(byte[] buffer) => _sha256.ComputeHash(_sha256.ComputeHash(buffer));
    }
}
