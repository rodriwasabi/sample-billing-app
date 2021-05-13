using System.Collections.Generic;
using System.Threading.Tasks;
using BasicBilling.API.Data;
using BasicBilling.API.Model;

namespace BasicBilling.API.Services
{
    public interface IBillingService
    {
        Task<int> CreateBillsForPeriod(int period, string category);
        Task<Bill> PayBill(int clientId, int period, string category);
        Task<Bill> RevertBill(int billId);

        Task<List<Bill>> GetBillByFilter(SearchBillFilter filter );
    }
}