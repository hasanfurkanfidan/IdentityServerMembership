﻿using IdentityServer4;
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
               new IdentityResource(){Name="Roles",DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims=new[]{"role" } },
               new IdentityResource(){Name="Email",DisplayName="Email",Description="Email",UserClaims=new []{"email" } },
               new IdentityResource(){Name="UserName",DisplayName="UserName",Description="UserName",UserClaims=new []{"name" } },

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
                    ,"CountryAndCity","Roles","Email"
                    },
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 2*60*60,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    SlidingRefreshTokenLifetime =Convert.ToInt32( (DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds),
                    RequireConsent = false,


                },
                  new Client()
                {
                    ClientId="Client2-Mvc",
                    RequirePkce=false,
                    ClientName ="Client MVC APP" ,
                    PostLogoutRedirectUris = new List<string>{ "https://localhost:5004/signout-callback-oidc" },
                    ClientSecrets = new List<Secret>{new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>(){ "https://localhost:5004/signin-oidc" },
                    AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,"api1.read",IdentityServerConstants.StandardScopes.OfflineAccess
                    ,"CountryAndCity","Roles"
                    },
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 2*60*60,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    SlidingRefreshTokenLifetime =Convert.ToInt32( (DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds),
                    RequireConsent = false,


                },
                  new Client()
                  {
                      ClientId="Clientjs",
                      RequireClientSecret = false,
                      AllowedGrantTypes  = GrantTypes.Code,
                      ClientName = "JsClientAngular",
                      AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,"api1.read",IdentityServerConstants.StandardScopes.OfflineAccess
                    ,"CountryAndCity","Roles","Email"
                    },
                      RedirectUris = {"http://localhost:4200/callback" },
                      AllowedCorsOrigins = { "http://localhost:4200" },
                      PostLogoutRedirectUris = { "http://localhost:4200" }
                  },
                       new Client()
                {
                    ClientId="Client1-ResourceOwner-Mvc",
                    RequirePkce=false,
                    ClientName ="Client MVC APP" ,
                    ClientSecrets = new List<Secret>{new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,"api1.read",IdentityServerConstants.StandardScopes.OfflineAccess
                    ,"CountryAndCity","Roles","Email","UserName"
                    },
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 2*60*60,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    SlidingRefreshTokenLifetime =Convert.ToInt32( (DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds),


                },
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
                    Claims =new List<Claim>
                    {
                        new Claim("given_name","Furkan"),
                        new Claim("family_name","Fidan"),
                        new Claim("country","Türkiye"),
                        new Claim("city","İstanbul"),
                         new Claim("role","Admin")
                    }
                },

            };
        }
       
    }
}
