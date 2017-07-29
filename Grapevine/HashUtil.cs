using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Grapevine
{
    public static class HashUtil
    {
        private static HashAlgorithm _sha256 = SHA256.Create();
        public static byte[] ComputeSHA256(params byte[][] args) => ComputeSHA256(args.SelectMany(b => b).ToArray());
        public static byte[] ComputeSHA256(byte[] buffer) => _sha256.ComputeHash(buffer);
    }
}
