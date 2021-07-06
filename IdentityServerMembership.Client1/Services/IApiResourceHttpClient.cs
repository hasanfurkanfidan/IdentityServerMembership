using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServerMembership.Client1.Services
{
    public interface IApiResourceHttpClient
    {
        Task<HttpClient> GetHttpClientAsync();
    }
}
