using System;
using System.Collections.Generic;

namespace BillingoApi.Models
{
    public class Invoice
    {
        public long Uid { get; set; }
        public long BlockUid { get; set; }

        public string InvoiceNo { get; set; }
        public string ConnectedInvoiceNo { get; set; }

        public DateTimeOffset Date { get; set; }
        public DateTimeOffset FulfillmentDate { get; set; }
        public DateTimeOffset DueDate { get; set; }

        public decimal Total { get; set; }
        public decimal TotalPaid { get; set; }

        public string Comment { get; set; }
        public string Currency { get; set; }
        public string TemplateLangCode { get; set; }
        public int ElectronicInvoice { get; set; }
        public long ClientUid { get; set; }
        public string Prefix { get; set; }
        public double NetTotal { get; set; }
        public string TypeString { get; set; }
        public Client Client { get; set; }
        public List<Item> Items { set; get; }
        public Payment PaymentMethod { get; set; }
    }
}
