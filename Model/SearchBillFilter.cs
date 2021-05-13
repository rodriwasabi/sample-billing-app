namespace BasicBilling.API.Model
{
    public class SearchBillFilter
    {
        public int? ClientId { get; set; }
        public int? Period { get; set; }
        public string? Category { get; set; }
        public string? Status { get; set; }
    }
}