using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ar.AzureFunctions.Data;
using Dapper;
using Microsoft.Azure.Services.AppAuthentication;

namespace ar.AzureFunctions.Commands
{
    public class GetProductCommand : IGetProductCommand
    {
        private static string _connectionString = Environment.GetEnvironmentVariable("ProductionConnectionString");
        public IDictionary<string, string> Criteria { set; private get; }

        public IEnumerable<Product> Result {get;private set;}

        public void Execute()
        {
            //build filter 
            var sql = @"select product_id as ProductId, product_name as ProductName, model_year as ModelYear, list_price as ListPrice, 
                                b.brand_id as BrandId, b.brand_name as BrandName, c.category_id as CategoryId, c.category_name as CategoryName 
                        from production.products p 
                        join production.brands b on p.brand_id = b.brand_id 
                        join production.categories c on p.category_id = c.category_id
                    where 1=1";

            string productId, brandId, categoryId, productName, brandName, categoryName;
            var parameters = new Dictionary<string,object>();
            Criteria.TryGetValue("productId",out productId);
            Criteria.TryGetValue("productName",out productName);
            Criteria.TryGetValue("brandId",out brandId);
            Criteria.TryGetValue("brandName",out brandName);
            Criteria.TryGetValue("categoryId",out categoryId);
            Criteria.TryGetValue("categoryName",out categoryName);

            if (!string.IsNullOrWhiteSpace(productName))
            {
                parameters.Add("productName",productName);
                sql += $" and p.product_name like '%' + @productName + '%'";
            }
            if (!string.IsNullOrWhiteSpace(productId))
            {
                //check for multiple products
                if (productId.Contains(","))
                {
                    parameters.Add("productId",productId.Split(','));
                    sql += $" and p.product_id in @productId";
                }
                else
                {
                    parameters.Add("productId",productId);
                    sql += $" and p.productId = @productId";
                }
            }

            if (!string.IsNullOrWhiteSpace(brandName))
            {
                parameters.Add("brandName",brandName);
                sql += $" and b.brand_name like '%' + @brandName + '%'";
            }
            if (!string.IsNullOrWhiteSpace(brandId))
            {
                //check for multiple brands
                if (brandId.Contains(","))
                {
                    parameters.Add("brandId",brandId.Split(','));
                    sql += $" and b.brand_id in @brandId";
                }
                else
                {
                    parameters.Add("brandId",brandId);
                    sql += $" and b.brand_id = @brandId";
                }
            }

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                parameters.Add("categoryName",categoryName);
                sql += $" and c.category_name like '%' + @categoryName + '%'";
            }
            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                //check for multiple categories
                if (categoryId.Contains(","))
                {
                    parameters.Add("categoryId",categoryId.Split(','));
                    sql += $" and c.category_id in @categoryId";
                }
                else
                {
                    parameters.Add("categoryId",categoryId);
                    sql += $" and c.category_id = @categoryId";
                }
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                connection.AccessToken = azureServiceTokenProvider.GetAccessTokenAsync("https://database.windows.net/").Result;
                //check for params
                if (parameters.Count() > 0)
                {
                    DynamicParameters dbParams = new DynamicParameters();
                    dbParams.AddDynamicParams(parameters);
                    Result = connection.Query<Product,Brand,Category,Product>(sql,(product,brand,category) => 
                    {
                        product.Brand = brand;
                        product.Category = category;
                        return product;
                    }, dbParams, splitOn: "BrandId,CategoryId").ToList();
                }
                else
                {
                    Result = connection.Query<Product,Brand,Category,Product>(sql, (product,brand,category) =>
                    {
                        product.Brand = brand;
                        product.Category = category;
                        return product;
                    }, splitOn: "BrandId,CategoryId").ToList();
                }
            }
        }
    }
}