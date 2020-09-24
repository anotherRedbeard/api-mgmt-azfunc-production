using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ar.AzureFunctions.Commands;
using ar.AzureFunctions.Data;

namespace ar.AzureFunctions.Services
{
    public class ProductionService : IProductionService
    {
        private IGetProductCommand _getProductCommand;

        public ProductionService(IGetProductCommand getProductCommand)
        {
            _getProductCommand = getProductCommand;
        }

        public async Task<IEnumerable<IProduct>> GetProduct(IDictionary<string,string> filterCriteria)
        {
            //check filter criteria format and parse into SWDVolumeByWellCriteria
            _getProductCommand.Criteria = filterCriteria;

            //run commands to get data
            List<ICommandWithResult<IDictionary<string,string>,IEnumerable<IProduct>>> commands = new List<ICommandWithResult<IDictionary<string,string>,IEnumerable<IProduct>>>();
            commands.Add(_getProductCommand);

            var results = Enumerable.Empty<IProduct>();
            Parallel.ForEach(commands,x => {
                x.Execute();
                results = results.Concat(x.Result);
            });

            return results;
        }
    }
}