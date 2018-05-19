namespace BillingoApi.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string LangCode { get; set; }
        public string Name { get; set; }
        public decimal AdvancePaid { get; set; }
    }
}
