using IdentityServerMembership.AuthServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerMembership.AuthServer.Services
{
    public class CustomUserRepository : ICustomUserRepository
    {
        private readonly CustomDbContext _context;
        public CustomUserRepository(CustomDbContext context)
        {
            _context = context;
        }
        public async Task<CustomUser> FindByEmailAsync(string email)
        {
            using (_context)
            {
                return await _context.CustomUsers.Where(p => p.Email == email).FirstOrDefaultAsync();
            }

        }

        public async Task<CustomUser> FindByIdAsync(int id)
        {
            using (_context)
            {
                return await _context.CustomUsers.Where(p => p.Id == id).FirstOrDefaultAsync();
            }
        }

        public async Task<bool> ValidateAsync(string email, string password)
        {
            using (_context)
            {
                var user = await _context.CustomUsers.Where(p => p.Email == email && p.Password == password).FirstOrDefaultAsync();
                if (user==null)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
