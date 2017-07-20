using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using Grapevine.Blockchain;

namespace Grapevine
{
    public static class Extensions
    {
        // https://stackoverflow.com/a/115034
        public static string ToISO8601(this DateTime dt) => dt.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
        public static uint ToEpoch(this DateTime dt) => (uint)((dt - new DateTime(1970, 1, 1)).TotalSeconds);
        public static Hash ToHash(this string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (var i = 0; i < hex.Length; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static Hash GetMerkleRoot(this IEnumerable<Hash> nodes)
        {
            var exterior = new Queue<Hash>(nodes);
            while (exterior.Count > 1)
            {
                var interior = new Queue<Hash>();
                while (exterior.Count > 1)
                {
                    using (var hasher = SHA256.Create())
                    using (var ms = new MemoryStream())
                    {
                        using (var bw = new BinaryWriter(ms))
                        {
                            bw.Write((byte[])exterior.Dequeue());
                            bw.Write((byte[])exterior.Dequeue());
                        }
                        interior.Enqueue(hasher.ComputeHash(ms.ToArray()));
                    }
                }
                if (exterior.Count == 1)
                    interior.Enqueue(exterior.Dequeue());
                exterior = interior;
            }
            return exterior.Dequeue();
        }
    }
}
