using IdentityServerMembership.Api2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerMembership.Api2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        [HttpGet,Authorize]
        public async Task<IActionResult> GetPictures()
        {
            var pictures = new List<Picture>() { new Picture() { Id = Guid.NewGuid(), ImageUrl = "abc.com" }, new Picture() { Id = Guid.NewGuid(), ImageUrl = "abd.com" } };
            return Ok(pictures);
        }
    }
}
