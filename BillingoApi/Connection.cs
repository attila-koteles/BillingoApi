using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BillingoApi
{
    public class Connection
    {
        string publicKey;
        string privateKey;

        const int LeeWay = 60;
        public const string ApiEndpoint = "https://www.billingo.hu/api";

        public Connection(string publicKey, string privateKey)
        {
            this.publicKey = publicKey;
            this.privateKey = privateKey;
        }

        /// <summary>
        /// The token in the HttpClient request header lives only for 60 seconds
        /// So don't store this reference, but rather create a new for every request you make
        /// </summary>
        /// <returns></returns>
        public HttpClient GetClient()
        {
            var client = new HttpClient();
            client.
                DefaultRequestHeaders.
                Accept.
                Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetNewToken());
            return client;
        }

        public async Task<string> GetAsync(string url)
        {
            return await GetClient().GetStringAsync(ApiEndpoint + url);
        }

        public static string MD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                return Encoding.ASCII.GetString(result);
            }
        }

        public string GetNewToken(long unixTimeStamp = 0)
        {
            if (unixTimeStamp == 0)
                unixTimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();

            var symmetricKey = Encoding.ASCII.GetBytes(privateKey);
            var payload = new JwtPayload
            {
                ["sub"] = publicKey,
                ["iat"] = unixTimeStamp - LeeWay,
                ["exp"] = unixTimeStamp + LeeWay,
                ["iss"] = "cli",
                ["nbf"] = unixTimeStamp - LeeWay,
                ["jti"] = MD5Hash(publicKey + unixTimeStamp)
            };

            var header = new JwtHeader(
                new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256)
                );

            var token = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.WriteToken(token);
            return jwt;
        }
    }
}
