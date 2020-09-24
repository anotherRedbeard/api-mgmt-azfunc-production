namespace ar.AzureFunctions.Data
{
    public class Product : IProduct
    {
        public int ProductId {get;set;}
        public string ProductName {get;set;}
        public short ModelYear {get;set;}
        public decimal ListPrice {get;set;}
        public IBrand Brand {get;set;}
        public ICategory Category {get;set;}
    }
}