using System;

namespace BasicBilling.API.Data
{
    public class Bill
    {
        public int BillId { get; set; }

        public int Period { get; set; }

        public int ClientId { get; set; }

        public string Category { get; set; }  // WATER, ELECTRICITY, SEWER

        public decimal Amount { get; set; }

        public string Status { get; set; } // PENDING, PAID

        public DateTime Updated { get; set; }

        public Client Client { get; set; }
    }
}