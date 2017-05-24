﻿using ProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using System.Web;

/*

Open your startup project's properties (Project->{ProjectName} Properties... from the main menu or right click your 
project in the Solution Explorer and choose Properties), then navigate to the Web tab and under Start Action choose 
Don't open a page. Wait for a request from an external application.

*/

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