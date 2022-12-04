using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using WebApplication4.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebApplication4.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<MyIdentityUser> _signInManager;
        private readonly UserManager<MyIdentityUser> _userManager;
        public HomeController(ILogger<HomeController> logger, SignInManager<MyIdentityUser> signInManager, UserManager<MyIdentityUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Home");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) return StatusCode(403);

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo =
                { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
            if (result == SignInResult.Success)
                return LocalRedirect("~/");
            else
            {
                var user = new MyIdentityUser
                {
                    UserName = info.Principal.FindFirst(ClaimTypes.Name).Value.Replace(" ", String.Empty),
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    FullName = info.Principal.FindFirst(ClaimTypes.Name).Value
                };

                var identityResult = await _userManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    identityResult = await _userManager.AddLoginAsync(user, info);
                    if (identityResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index");
                    }
                }

                return RedirectToAction("Index");
            }
        }
    }
}