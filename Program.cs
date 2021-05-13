using BasicBilling.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BasicBilling.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();


            var host = CreateHostBuilder(args).Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<BillingDbContext>();
                context.Database.EnsureCreated();

                var clientCount = context.Clients.CountAsync().GetAwaiter().GetResult();
                if(clientCount == 0)
                {
                    context.Clients.Add(new Client{
                        Name = "Josh Wallace"
                    });
                    context.Clients.Add(new Client{
                        Name = "Peter Mclure"
                    });
                    context.Clients.Add(new Client{
                        Name = "Martha Smith"
                    });
                    context.SaveChanges();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
