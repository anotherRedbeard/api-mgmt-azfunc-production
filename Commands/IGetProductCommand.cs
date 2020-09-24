using System.Collections.Generic;
using ar.AzureFunctions.Data;

namespace ar.AzureFunctions.Commands
{
    public interface IGetProductCommand : ICommandWithResult<IDictionary<string,string>,IEnumerable<Product>>
    {
        
    }
}