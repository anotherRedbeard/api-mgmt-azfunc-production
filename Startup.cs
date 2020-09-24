using ar.AzureFunctions.Commands;
using ar.AzureFunctions.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(ar.AzureFunctions.Production.Startup))]
namespace ar.AzureFunctions.Production
{
    // Implement IWebJobStartup interface.
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //builder.Services.AddSingleton<OpenInvoiceBuyerClient>();

            builder.Services.AddTransient<IProductionService, ProductionService>();
            builder.Services.AddTransient<IGetProductCommand, GetProductCommand>();
        }
    }
}
