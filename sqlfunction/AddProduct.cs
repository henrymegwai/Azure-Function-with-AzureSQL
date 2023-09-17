using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace sqlfunction
{
    public static class AddProduct
    {
        [FunctionName("AddProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Request received by function and it is currently processing");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            Product productData = JsonConvert.DeserializeObject<Product>(requestBody);

            if(productData != null)
            {
                products.Add(productData);
            }

            return new OkObjectResult(GetProducts());
        }

        private static List<Product> products = new();

        /// <summary>
        /// This is to help connect to Azure SQL Database on Azure
        /// </summary>
        /// <returns></returns>
        private static SqlConnection GetConnection()
        {
            // Not best best practice to hard code your connection string
            string connectionString = "";
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Return list of Products
        /// </summary>
        /// <returns></returns>
        private static List<Product> GetProducts()
        {
            return products;
        }
    }
}
