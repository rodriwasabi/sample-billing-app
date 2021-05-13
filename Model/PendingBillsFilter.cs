namespace BasicBilling.API.Model
{
    public class PendingBillsFilter
    {
        public int? ClientId { get; set; }
        public string? Category { get; set; }
    }
}