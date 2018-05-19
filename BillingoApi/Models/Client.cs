namespace BillingoApi.Models
{
    public class Client
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Taxcode { get; set; }
        public Address BillingAddress { get; set; }
        public Bank Bank { get; set; }
    }
}
