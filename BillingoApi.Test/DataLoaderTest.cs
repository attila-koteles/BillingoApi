using BillingoApi.Core;
using BillingoApi.Models;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BillingoApi.Test
{
    public class DataLoaderTest
    {
        DataLoader<Block> dataloader;

        public DataLoaderTest()
        {
            var expectedBlockResults = "{\"success\":true,\"type\":\"invoice_blocks\",\"data\":[{\"id\":16365521,\"attributes\":{\"name\":\"Sz\u00e1ml\u00e1k\",\"prefix\":\"\",\"uid\":16365521}},{\"id\":4599810,\"attributes\":{\"name\":\"SOS BLOCK\",\"prefix\":\"SOS\",\"uid\":4599810}}]}";

            var mock = new Mock<Connection>("", "");
            mock
                .Setup(connection => connection.GetAsync("/invoices/blocks?page=1&max_per_page=50"))
                .Returns(Task.FromResult(expectedBlockResults));

            dataloader = new DataLoader<Block>(mock.Object, "/invoices/blocks");
        }

        [Fact]
        public void FetchAllAsyncTest()
        {
            var blocks = dataloader.FetchAllAsync(50).Result;
            Assert.Equal(2, blocks.Count);

            var expected = new List<Block>
            {
                new Block
                {
                    Name = "Számlák",
                    Prefix = "",
                    Uid = 16365521
                },
                new Block
                {
                    Name = "SOS BLOCK",
                    Prefix = "SOS",
                    Uid = 4599810
                },
            };

            // very simple equality check
            Assert.Equal(
                JsonConvert.SerializeObject(expected),
                JsonConvert.SerializeObject(blocks)
                );
        }
    }
}
