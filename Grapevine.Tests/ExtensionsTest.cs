using System;
using System.Collections.Generic;
using System.Text;
using Grapevine;
using Xunit;

namespace Grapevine.Tests
{
    public class ExtensionsTest
    {
        [Fact]
        public void TestToHash()
        {
            Assert.Equal(new byte[] { 0 }, (byte[])"00".ToHash());
        }
    }
}
