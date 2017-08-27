using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Grapevine.Blockchain
{
    public class WalletKey
    {
        public WalletKey(ECDsa dsa)
        {
            DSA = dsa;

            Address = ConstructAddress();
        }

        public WalletKey(ECParameters ecp)
            : this(ECDsa.Create(ecp))
        { }

        public ECDsa DSA { get; private set; }

        public byte[] PrivateKey => DSA.ExportParameters(true).D;
        public ECPoint PublicKeyPoint => DSA.ExportParameters(false).Q;
        public byte[] PublicKey
        {
            get
            {
                var pubKeyPt = PublicKeyPoint;
                using (var ms = new MemoryStream())
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write((byte)0x04);

                    var xBytes = pubKeyPt.X;
                    Array.Resize(ref xBytes, 32);
                    bw.Write(xBytes);

                    var yBytes = pubKeyPt.Y;
                    Array.Resize(ref yBytes, 32);
                    bw.Write(yBytes);

                    return ms.ToArray();
                }
            }
        }

        private string ConstructAddress()
        {
            // TODO: FIX, but we want 160-bit hash
            var publicKeyHash = SHA1.Create().ComputeHash(HashUtil.Compute(PublicKey));

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write((byte)AddressType.Normal);
                bw.Write((byte[])publicKeyHash);
                bw.Write(HashUtil.Compute(HashUtil.Compute(ms.ToArray())).Take(4).ToArray());

                return new Base58CheckCodec().Encode(ms.ToArray());
            }
        }

        public string Address
        {
            get;
            private set;
        }

        public static WalletKey Create()
        {
            using (var dsa = ECDsa.Create())
            {
                dsa.GenerateKey(ECCurve.CreateFromFriendlyName("SECP256k1"));
                return new WalletKey(dsa.ExportParameters(true));
            }
        }
    }
}
