using System;
using System.Collections.Generic;
using System.Text;
using Grapevine;
using Grapevine.Blockchain;
using Xunit;
using System.Security.Cryptography;
using System.Linq;

namespace Grapevine.Tests
{
    public class WalletTests
    {
        [Fact]
        public void TestSignature()
        {
            var wk = WalletKey.Create();

            const string signedMessage = "Wallet Test 2017";
            const string unsignedMessage = "Tallet West 1720";

            var signature = wk.DSA.SignData(Encoding.UTF8.GetBytes(signedMessage), HashAlgorithmName.SHA256);

            Assert.True(wk.DSA.VerifyData(Encoding.UTF8.GetBytes(signedMessage), signature, HashAlgorithmName.SHA256));
            Assert.False(wk.DSA.VerifyData(Encoding.UTF8.GetBytes(unsignedMessage), signature, HashAlgorithmName.SHA256));
            Assert.False(wk.DSA.VerifyData(Encoding.UTF8.GetBytes(signedMessage), signature, HashAlgorithmName.MD5));
        }
    }
}
