using System;
using Xunit;
using BillingoApi;

namespace BillingoApi.Test
{
    public class ConnectionTest
    {
        // Mock keys
        const string PublicKey = "62617b5002d913d147d1f9037621df09";
        const string PrivateKey = "53a7e6a55dffde571e5e3ce977c772d48da6d5a7bcb6d46263352847910a76d29606a7d98c01cb9c6f8b5841af1fab2fb594e48f3712e3361abcf1fc4c989dae";

        [Fact]
        public void TestGetNewToken()
        {
            var c = new Connection(PublicKey, PrivateKey);
            var timestamp = DateTimeOffset.Parse("2018-05-20 00:00:00").ToUnixTimeSeconds();
            var expected = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI2MjYxN2I1MDAyZDkxM2QxNDdkMWY5MDM3NjIxZGYwOSIsImlhdCI6MTUyNjc2NzE0MCwiZXhwIjoxNTI2NzY3MjYwLCJpc3MiOiJjbGkiLCJuYmYiOjE1MjY3NjcxNDAsImp0aSI6Ij8_Pz9YXHUwMDFmPzRTPz9vXHUwMDA2P1x1MDAwYj8ifQ.kBBN_ljeAqsy6fek6OexaWytFzStC4vhYmR6Z4VNcrM";
            Assert.Equal(expected, c.GetNewToken(timestamp));
        }
    }
}
