using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Linq;

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
            if (nodes.Count() == 0)
                return null;
            if (nodes.Count() == 1)
                return nodes.FirstOrDefault();

            var merkle = new Queue<Hash>();
            while (merkle.Count > 1)
            {
                // Create a copy of the last entry if we have an odd number of entries
                if (merkle.Count % 2 == 1)
                    merkle.Enqueue(new Hash(merkle.Last().Digest));

                // Start creating our next layer
                var nextMerkle = new Queue<Hash>();
                while (merkle.Count > 0)
                {
                    using (var ms = new MemoryStream())
                    using (var bw = new BinaryWriter(ms))
                    {
                        // We can assume merkle was even to start with
                        // => it will keep on being even if we always dequeue 2 at a time
                        bw.Write((byte[])merkle.Dequeue());
                        bw.Write((byte[])merkle.Dequeue());

                        nextMerkle.Enqueue(HashUtil.Compute(HashUtil.Compute(ms.ToArray())));
                    }

                }
                merkle = nextMerkle;
            }

            return merkle.Dequeue();
        }
    }
}
