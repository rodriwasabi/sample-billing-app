namespace BasicBilling.API.Model
{
    public class CreateBillRequest
    {
        public int Period { get; set; }

        public string Category { get; set; } 
    }
}