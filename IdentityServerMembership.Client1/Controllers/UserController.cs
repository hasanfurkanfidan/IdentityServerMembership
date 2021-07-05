using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServerMembership.Client1.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {

            return View();
        }
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookie1");
            await HttpContext.SignOutAsync("oidc");

        }
        public async Task<IActionResult> GetRefreshToken()
        {
            //tekrardan signin olacak
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            HttpClient httpClient = new HttpClient();
            var discovery = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");

            var refreshTokenRequest = new RefreshTokenRequest
            {
                ClientId = _configuration["OIdcClient:ClientId"],
                ClientSecret = _configuration["OIdcClient:ClientSecret"],
                RefreshToken = refreshToken,
                Address = discovery.TokenEndpoint,

            };
            var token = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);
            if (token.IsError)
            {
                //Yönlendirme yap
            }
            var tokens = new List<AuthenticationToken>
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = token.IdentityToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = token.AccessToken
                },
                 new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = token.RefreshToken
                },
                 new AuthenticationToken
                 {
                     Name = OpenIdConnectParameterNames.ExpiresIn,
                     Value = DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)
                 },
            };
            var authenticationResult = await HttpContext.AuthenticateAsync();

            var authProperties = authenticationResult.Properties;

            //Setting properties
            authProperties.StoreTokens(tokens);


            //ReSignin
            await HttpContext.SignInAsync("Cookie1", authenticationResult.Principal, authProperties);

            return RedirectToAction("Index");
        }
        [Authorize(Roles ="Admin")]
        public IActionResult AdminAction()
        {
            return View();
        }
        [Authorize(Roles = "Customer")]

        public IActionResult CustomerAction()
        {
            return View();
        }

    }
}
