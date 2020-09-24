namespace ar.AzureFunctions.Data
{
    public interface IProduct
    {
        int ProductId {get;set;}
        string ProductName {get;set;}
        short ModelYear {get;set;}
        decimal ListPrice {get;set;}
        IBrand Brand {get;set;}
        ICategory Category {get;set;}
    }
}