using System;
using Xunit;
using Vexilla.Client;
using System.Diagnostics;
using Xunit.Abstractions;

namespace Vexilla.Client.Tests
{
    public class HasherTest
    {
        private readonly ITestOutputHelper output;

        public HasherTest(ITestOutputHelper output)
        {
            this.output = output;
        }
        private String uuid = "b7e91cc5-ec76-4ec3-9c1c-075032a13a1a";
        private Double workingSeed = 0.11;
        private Double nonWorkingSeed = 0.22;


        [Fact]
        public void TestWorkingGradual()
        {
            VexillaHasher hasher = new VexillaHasher(this.workingSeed);

            Double result = hasher.hashString(this.uuid);

            output.WriteLine("result");
            output.WriteLine(result.ToString());

            Assert.True(hasher.hashString(this.uuid) <= 40);
        }

        [Fact]
        public void TestNonWorkingGradual()
        {
            VexillaHasher hasher = new VexillaHasher(this.nonWorkingSeed);

            Double result = hasher.hashString(this.uuid);

            output.WriteLine("result");
            output.WriteLine(result.ToString());

            Assert.True(hasher.hashString(this.uuid) >= 40);
        }
    }
}
