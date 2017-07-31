using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Grapevine
{
    public static class HashUtil
    {
        public static HashAlgorithm Algorithm { get; set; } = SHA256.Create();

        public static byte[] Compute(params byte[][] args) => Compute(args.SelectMany(b => b).ToArray());
        public static byte[] Compute(byte[] buffer) => Algorithm.ComputeHash(buffer);

        public static byte[] GrapeCompute(byte[] buffer) => Algorithm.ComputeHash(Algorithm.ComputeHash(buffer));
    }
}
