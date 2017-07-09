using ProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using System.Web;

// http://www.hanselman.com/blog/HowToRunBackgroundTasksInASPNET.aspx
// http://haacked.com/archive/2011/10/16/the-dangers-of-implementing-recurring-background-tasks-in-asp-net.aspx/
// https://www.hangfire.io/
// https://msdn.microsoft.com/en-us/library/ms227673.aspx

namespace ProductsApp.Controllers
{
    public class ProductsController : ApiController
    {
        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
        };

        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }

        public IHttpActionResult GetProduct(int id)
        {
            var product = products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IHttpActionResult RunAction()
        {
            HttpContent requestContent = Request.Content;
            string jsonContent = requestContent.ReadAsStringAsync().Result;
            //File.AppendAllText(@"C:\temp\1.txt", jsonContent + "\n");
            jsonContent = HttpUtility.UrlDecode(jsonContent);
            //File.AppendAllText(@"C:\temp\1.txt", jsonContent + "\n");
            
            return Ok(
                new { ok=1}
                );
        }
    }
}