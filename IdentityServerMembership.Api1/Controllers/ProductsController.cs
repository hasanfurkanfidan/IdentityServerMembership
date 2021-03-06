using IdentityServerMembership.Api1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerMembership.Api1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy ="ReadProduct")]
        public async Task<IActionResult> GetProducts()
        {
            var productList = new List<Product> { new Product { Id = 1, Name = "Product1" }, new Product { Id = 2, Name = "Product2" } };
            return Ok(productList);
        }
        [HttpPost]
        [Authorize(Policy = "UpdateOrCreate")]

        public async Task<IActionResult>AddProduct(Product product)
        {
            return Ok(product);
        }
    }
}
