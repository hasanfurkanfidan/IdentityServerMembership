using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityServerMembership.Client1.Models;
using Newtonsoft.Json;

namespace IdentityServerMembership.Client1.Controllers
{
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;
        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            var httpClient = new HttpClient();
            var discovery = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");
            var models = new List<ProductViewModel>();
            if (discovery.IsError)
            {
                //Error
            }
            var clientCredentialTokenRequest = new ClientCredentialsTokenRequest();
            clientCredentialTokenRequest.ClientId = _configuration["Client:ClientId"];

            clientCredentialTokenRequest.ClientSecret = _configuration["Client:ClientSecret"];
            clientCredentialTokenRequest.Address = discovery.TokenEndpoint;
            var token = await httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);
            if (token.IsError)
            {
                //Loglama
            }
            httpClient.SetBearerToken(token.AccessToken);
            var response = await httpClient.GetAsync("https://localhost:5006/api/products/getproducts");
            if (response.IsSuccessStatusCode)
            {
                var products = JsonConvert.DeserializeObject<List<ProductViewModel>>(await response.Content.ReadAsStringAsync());
                models = products;
            }
            return View(models);
        }
    }
}
