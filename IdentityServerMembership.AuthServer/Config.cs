using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace IdentityServerMembership.AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> {
                new ApiResource("ResourceApi1") {Scopes={"api1.read","api1.write","api1.update" } },
                new ApiResource("ResourceApi2"){Scopes = { "api2.write","api2.read","api2.update"} }
            };
        }
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("ap1.read","Api 1 için okuma izni"),
                new ApiScope("api1.write","Api 1 için yazma izni"),
                new ApiScope("api1.update","Api 1 için güncelleme izni"),
                new ApiScope("ap2.read","Api 2 için okuma izni"),
                new ApiScope("api2.write","Api 2 için yazma izni"),
                new ApiScope("api2.update","Api 2 için güncelleme izni"),

            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId = "Client1",
                    ClientName="Client 1",
                    ClientSecrets = new List<Secret>{new Secret("secret".Sha256()) { } },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api1.read","api2.write"}
                },
                new Client()
                {
                    ClientId = "Client2",
                    ClientName = "Client2",
                    ClientSecrets = new List<Secret>{new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"api2.read,api1.write"}
                }
            };
        }
    }
}
