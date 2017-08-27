using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Grapevine.Tests
{
    public class HashTests
    {
        private readonly ITestOutputHelper output;
        public HashTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void TestNullHash()
        {
            // Automatically 32 0-bytes
            var hash = new byte[32];
            output.WriteLine(((Hash)HashUtil.Compute(hash)).ToString());
        }

        [Fact]
        public void TestHashClass()
        {
            Assert.Equal(new byte[] { 0 }, new Hash(new byte[] { 0 }));
        }
    }
}
