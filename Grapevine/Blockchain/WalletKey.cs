using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Grapevine.Blockchain
{
    public class WalletKey
    {
        public WalletKey(ECDsa dsa)
        {
            DSA = dsa;
        }

        public WalletKey(ECParameters ecp)
            : this(ECDsa.Create(ecp))
        { }

        public ECDsa DSA { get; private set; }

        public byte[] PrivateKey => DSA.ExportParameters(true).D;
        public ECPoint PublicKey => DSA.ExportParameters(false).Q;

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
