using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace IdentityServerMembership.AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> {
                new ApiResource("ResourceApi1") {Scopes={"api1.read","api1.write","api1.update" },ApiSecrets=new[]{new Secret("secretapi.1".Sha256()) } },
                new ApiResource("ResourceApi2"){Scopes = { "api2.write","api2.read","api2.update"} }
            };
        }
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
               new  IdentityResources.OpenId(),//subId
               new IdentityResources.Profile(),
               new IdentityResource(){Name="CountryAndCity",DisplayName="CountryAndCity",Description = "Kullanıcının ülke ve şehir bilgisi"
              ,UserClaims = new[]{"country","city"}
               },
               new IdentityResource(){Name="Roles",DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims=new[]{"role" } }

            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("api1.read","Api 1 için okuma izni"),
                new ApiScope("api1.write","Api 1 için yazma izni"),
                new ApiScope("api1.update","Api 1 için güncelleme izni"),
                new ApiScope("api2.read","Api 2 için okuma izni"),
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
                    AllowedScopes = {"api2.read","api1.write"}
                },
                new Client()
                {
                    ClientId="Client1-Mvc",
                    RequirePkce=false,
                    ClientName ="Client MVC APP" ,
                    PostLogoutRedirectUris = new List<string>{ "https://localhost:5002/signout-callback-oidc" },
                    ClientSecrets = new List<Secret>{new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>(){ "https://localhost:5002/signin-oidc" },
                    AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,"api1.read",IdentityServerConstants.StandardScopes.OfflineAccess
                    ,"CountryAndCity","Roles"
                    },
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 2*60*60,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    SlidingRefreshTokenLifetime =Convert.ToInt32( (DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds),
                    RequireConsent = true,


                }
            };
        }
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId="1",
                    Username = "furkanfidan.job@gmail.com",
                    Password = "106673",
                    Claims = GetClaims("Furkan","Fidan","Türkiye","Tekirdağ",
                    "Admin")
                },

            };
        }
        public static List<Claim> GetClaims(string name, string surname, string country, string city,string role)
        {
            return new List<Claim>
            {
                new Claim("given_name",name),
                new Claim("family_name",surname),
                new Claim("country",country),
                new Claim("city",city),
                new Claim("role",role)
            };
        }
    }
}
