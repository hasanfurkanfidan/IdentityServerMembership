using IdentityModel.Client;
using IdentityServerMembership.Client1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServerMembership.Client1.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var client = new HttpClient();

                var discovery = await client.GetDiscoveryDocumentAsync(_configuration["AuthServerUrl"]);

                if (discovery.IsError)
                {
                    //Hata yakala
                }

                var passwordTokenRequest = new PasswordTokenRequest();
                passwordTokenRequest.Address = discovery.TokenEndpoint;
                passwordTokenRequest.ClientId = _configuration["ClientResourceOwner:ClientId"];
                passwordTokenRequest.ClientSecret = _configuration["ClientResourceOwner:ClientSecret"];
                passwordTokenRequest.UserName = model.Email;
                passwordTokenRequest.Password = model.Password;


                var result = await client.RequestPasswordTokenAsync(passwordTokenRequest);
                if (result.IsError)
                {
                    //Hata Logla
                    ModelState.AddModelError("", result.Error);
                    return View();
                }

                var userInfoRequest = new UserInfoRequest();
                userInfoRequest.Token = result.AccessToken;
                userInfoRequest.Address = discovery.UserInfoEndpoint;

                var userInfoResult = await client.GetUserInfoAsync(userInfoRequest);

                if (userInfoResult.IsError)
                {
                    //Log
                }

                //Claims saved

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfoResult.Claims, "Cookie1");

                var claimPrincipal = new ClaimsPrincipal(claimsIdentity);

                var authProperties = new AuthenticationProperties();

                authProperties.StoreTokens(new List<AuthenticationToken>
                {
                    new AuthenticationToken
                    {
                        Name= OpenIdConnectParameterNames.AccessToken,
                        Value = result.AccessToken
                    },
                    new AuthenticationToken
                    {
                        Name = OpenIdConnectParameterNames.RefreshToken,
                        Value = result.RefreshToken
                    },
                      new AuthenticationToken
                    {
                        Name = OpenIdConnectParameterNames.IdToken,
                        Value = result.IdentityToken
                    },
                         new AuthenticationToken
                    {
                        Name = OpenIdConnectParameterNames.ExpiresIn,
                        Value = DateTime.UtcNow.AddSeconds(result.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)
                    },
                }); ;

                await HttpContext.SignInAsync(claimPrincipal,authProperties);
            }
            return View();
        }
    }
}
