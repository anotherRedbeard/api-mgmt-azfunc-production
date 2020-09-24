using System.Collections.Generic;
using System.Threading.Tasks;
using ar.AzureFunctions.Data;

namespace ar.AzureFunctions.Services
{
    public interface IProductionService
    {
        Task<IEnumerable<IProduct>> GetProduct(IDictionary<string,string> filterCriteria);
    }
}