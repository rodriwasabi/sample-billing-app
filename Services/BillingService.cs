using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BasicBilling.API.Data;
using BasicBilling.API.Model;
using Microsoft.EntityFrameworkCore;

namespace BasicBilling.API.Services
{
    public class BillingService : IBillingService
    {
        public BillingDbContext _context { get; }
        public BillingService(BillingDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateBillsForPeriod(int period, string category)
        {
            var clients = await _context.Clients.Select(p => p.ClientId).ToListAsync();

            // Validates if period is valid. Format: yyyymm
            if(!Regex.IsMatch(period.ToString(), "^\\d{6}$"))
            {
                throw new ApplicationException("Period does not have the format: yyyymm");
            }

            if(category != "WATER" && category != "ELECTRICITY" && category != "SEWER")
                throw new ApplicationException("Category is not valid");

            var billsForPeriod = await _context.Bills.Where(p => p.Period == period 
                && p.Category == category
                && clients.Any(c => c == p.ClientId))
                .ToListAsync();

            foreach (var client in clients)
            {
                if(!billsForPeriod.Any(p => p.ClientId == client))
                {
                    Random rand = new Random(50);
                    
                    var bill = new Bill
                    {
                        Period = period,
                        ClientId = client,
                        Category = category,
                        Amount = Convert.ToDecimal(rand.NextDouble() * 70),
                        Status = "PENDING",
                        Updated = DateTime.Now
                    };
                    await _context.Bills.AddAsync(bill);
                }                
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<Bill> PayBill(int clientId, int period, string category)
        {
            Validate(clientId, period, category);

            var bill = await _context.Bills.FirstOrDefaultAsync
            (
                p => p.ClientId == clientId &&
                    p.Category == category.ToUpper() && 
                    p.Period == period
            );

            if (bill == null)
                throw new ApplicationException("Requested bill was not found");
            else if (bill.Status != "PAID")
            {
                bill.Status = "PAID";
                bill.Updated = DateTime.Now;

                _context.Update(bill);
                await _context.SaveChangesAsync();
            }
            return bill;
        }

        public async Task<Bill> RevertBill(int billId)
        {
            var bill = await _context.Bills.FirstOrDefaultAsync(p => p.BillId == billId);
            if (bill != null)
            {
                bill.Status = "PENDING";
                bill.Updated = DateTime.Now;
                
                _context.Update(bill);
                await _context.SaveChangesAsync();
            }
            return bill;
        }

        public async Task<List<Bill>> GetBillByFilter(SearchBillFilter filter)
        {
            var bills = _context.Bills.AsQueryable();

            if (filter.ClientId.HasValue)
                bills = bills.Where(p => p.ClientId == filter.ClientId);
            
            if (filter.Period.HasValue)
                bills = bills.Where(p => p.Period == filter.Period);

            if(filter.Status != null)
                bills = bills.Where(p => p.Status == filter.Status);

            if (filter.Category != null)
                bills = bills.Where(p => p.Category == filter.Category);

            return await bills.ToListAsync();
        }

        private void Validate(int clientId, int period, string category)
        {
            if(clientId <= 0)
                throw new ApplicationException("Client id is not valid");

            if(period <= 0
                || !Regex.IsMatch(period.ToString(), "^\\d{6}$"))
                throw new ApplicationException("Period is invalid");

            if(string.IsNullOrEmpty(category))
                throw new ApplicationException("Category is invalid");
        }
    }
}