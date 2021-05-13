using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BasicBilling.API.Data
{
    public class BillingDbContext : DbContext
    {
        public BillingDbContext(DbContextOptions<BillingDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        
        // protected override void OnConfiguring(DbContextOptionsBuilder options)
        //     => options.UseSqlite("Data Source=Billing.db");
    }   
}