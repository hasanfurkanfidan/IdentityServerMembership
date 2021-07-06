using IdentityServerMembership.AuthServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerMembership.AuthServer.Services
{
    public interface ICustomUserRepository
    {
        Task<bool> ValidateAsync(string email, string password);
        Task<CustomUser> FindByIdAsync(int id);
        Task<CustomUser> FindByEmailAsync(string email);
    }
}
