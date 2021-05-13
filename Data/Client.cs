using System.Collections.Generic;

namespace BasicBilling.API.Data
{
    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }

        public List<Bill> Bills { get; set; }
    }
}