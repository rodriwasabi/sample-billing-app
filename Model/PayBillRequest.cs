namespace BasicBilling.API.Model
{
    public class PayBillRequest
    {
        public int ClientId { get; set; }

        public int Period { get; set; }

        public string Category { get; set; }
    }
}
