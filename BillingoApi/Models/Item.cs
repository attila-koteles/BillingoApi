namespace BillingoApi.Models
{
    public class Item
    {
        public string Description { get; set; }
        public double NetUnitPrice { get; set; }
        public double NetRowPrice { get; set; }

        public decimal GrossRowPrice { get; set; } // always fixed-point

        public double VatValue { get; set; }

        public decimal Qty { get; set; }

        public int VatId { get; set; }
        public string ItemComment { get; set; }
        public Vat VatObject { get; set; }
    }
}
