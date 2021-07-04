using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerMembership.Client1.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            
            return View();
        }
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookie1");
            await HttpContext.SignOutAsync("oidc");

        }
    }
}
