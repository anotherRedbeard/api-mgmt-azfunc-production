using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ar.AzureFunctions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace ar.AzureFunctions.Production
{
    public class GetProduct
    {
        private IProductionService _productionService;

        public GetProduct(IProductionService productionService)
        {
            _productionService = productionService;
        }

        [FunctionName("GetProduct")]
        public async Task<IActionResult> Run( [HttpTrigger(AuthorizationLevel.Function, "get", Route = "production/product")] HttpRequest req, ILogger log)
        {
            log.LogInformation("production/product processing a request");

            try
            {
                var criteria = req.GetQueryParameterDictionary();
                var results = await _productionService.GetProduct(criteria);
                return (ActionResult) new OkObjectResult(results);
            }
            catch (Exception ex)
            {
                string error = $"Error occurred processing request on production/product:\r\n{ex.Message}";
                log.LogError(ex,error);
                return new BadRequestObjectResult(error);
            }
        }
    }
}