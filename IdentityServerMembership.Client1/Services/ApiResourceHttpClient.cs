using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServerMembership.Client1.Services
{
    public class ApiResourceHttpClient : IApiResourceHttpClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApiResourceHttpClient(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<HttpClient> GetHttpClientAsync()
        {
            var httpClient = new HttpClient();
            var accesToken =await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            httpClient.SetBearerToken(accesToken);

            return httpClient;
        }
    }
}
