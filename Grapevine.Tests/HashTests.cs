using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Grapevine;

namespace Grapevine.Tests
{
    public class HashTests
    {
        [Fact]
        public void TestNullHash()
        {
            var hash = new byte[32];
            HashUtil.Compute(hash);
        }
    }
}
