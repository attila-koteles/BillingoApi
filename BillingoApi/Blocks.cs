using BillingoApi.Core;
using BillingoApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillingoApi
{
    public class Blocks
    {
        readonly Connection connection;

        public Blocks(string publicKey, string privateKey)
        {
            connection = new Connection(publicKey, privateKey);
        }

        public async Task<List<Block>> LoadAllAsync(int limit = 50)
        {
            var l = new DataLoader<Block>(connection, "/invoices/blocks");
            return await l.FetchAllAsync(limit);
        }
    }
}
