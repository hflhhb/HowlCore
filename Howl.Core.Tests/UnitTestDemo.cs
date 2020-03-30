using System;
using Xunit;
using Howl.Core.Extensions;

namespace Howl.Core.Tests
{
    public class UnitTestDemo
    {
        [Fact]
        public void TestDemo()
        {
            var x = 1;
            Assert.True(x == 1);
        }

        [Fact]
        public void Test_Extensions_To()
        {
            var x = "1";
            var y = x.To<int>();

            Assert.True(y == 1);
        }
    }
}
