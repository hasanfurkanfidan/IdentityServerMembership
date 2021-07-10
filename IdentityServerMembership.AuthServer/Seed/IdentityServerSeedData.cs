using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerMembership.AuthServer.Seed
{
    public static class IdentityServerSeedData
    {
        public static void Seed(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var item in Config.GetClients())
                {
                    context.Clients.Add(item.ToEntity());
                }
            }
            if (!context.ApiResources.Any())
            {
                foreach (var item in Config.GetApiResources())
                {
                    context.ApiResources.Add(item.ToEntity());
                }
            }
            if (!context.ApiScopes.Any())
            {
                foreach (var item in Config.GetApiScopes())
                {
                    context.ApiScopes.Add(item.ToEntity());
                }
            }
            if (!context.IdentityResources.Any())
            {
                foreach (var item in Config.GetIdentityResources())
                {
                    context.IdentityResources.Add(item.ToEntity());
                }
            }
            context.SaveChanges();
        }
    }
}
