using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Grapevine.Blockchain
{
    public class BlockHeader
    {
        private UInt32 BlockVersion { get; set; } = 0;
        public Hash PreviousBlock { get; set; }
        public Hash TxMerkleRoot { get; set; }
        public DateTime Timestamp { get; set; }
        public UInt32 CompactTarget { get; set; }
        public UInt32 Nonce { get; set; }

        public Hash GenerateProof()
        {
            using (var hasher = SHA256.Create())
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write((UInt32)BlockVersion);
                    bw.Write((byte[])PreviousBlock);
                    bw.Write((byte[])TxMerkleRoot);
                    bw.Write((UInt32)Timestamp.ToEpoch());
                    bw.Write((UInt32)CompactTarget);
                    bw.Write((UInt32)(++Nonce));                    
                }

                return (Hash)hasher.ComputeHash(ms.ToArray());
            }
        }
    }
}
