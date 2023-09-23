using SampleShop;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;


using SampleShop.Interfaces;
using SampleShop.Services;
using SampleShop.Utilities;
using SampleShop.Queries;

[assembly: WebJobsStartup(typeof(Startup))]
namespace SampleShop
{
    public class Startup : IWebJobsStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            
            services.AddSingleton<IDatabase>(s =>
            {
                return DatabaseFactory.CreateDatabase();
            });

            services.AddScoped<AddOrderQuery>();
            services.AddScoped<DeleteOrderQuery>();
            services.AddScoped<GetAllItemsQuery>();
            services.AddScoped<GetAllOrdersQuery>();
            services.AddScoped<GetOrderByIdQuery>();
            


            services.AddScoped<IItemsService, ItemsService>();
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ILogger, Logger>();
        }

        public void Configure(IWebJobsBuilder builder)
        {
            ConfigureServices(builder.Services);
        }

    }
}
