using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Infrastructure;
using Application.UseCases.Auth;
using Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using AuthenticationService = Application.Infrastructure.AuthenticationService;

namespace BlazorServerApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class Login2Model : PageModel
    {
        //private EntityDataService entityDataService;
        private AuthenticationService authenticationService;
        private readonly ILogger<Login2Model> logger;

        public Login2Model(ILogger<Login2Model> _logger,
            AuthenticationService _authenticationService)
        {
            authenticationService = _authenticationService;
            logger = _logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }


        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            ReturnUrl = returnUrl;
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            try
            {
                if (ModelState.IsValid)
                {
                    UserLoginCommand request = new UserLoginCommand
                    {
                        UserName = Input.Email,
                        Password = Input.Password,
                    };

                    var rsp = await authenticationService.Login(request);

                    if (rsp.IsSuccessStatusCode)
                    {
                        logger.LogInformation("User logged in.");

                        var tokenDetails = rsp.Content.Response;

                        var claims = new List<Claim>();

                        claims.Add(new Claim(ClaimTypes.Email, Input.Email));
                        claims.Add(new Claim(ClaimTypes.Name, Input.Email));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, Input.Email));
                        claims.Add(new Claim(ConfigKeys.ACCESS_TOKEN_KEY, tokenDetails.Token));


                        var claimsIdentity =
                            new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = Input.RememberMe,
                            RedirectUri = this.Request.Host.Value,
                            ExpiresUtc = DateTime.UtcNow.AddHours(10)
                        };

                        logger.LogInformation("Signing in the user");

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity), authProperties);

                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        var data = rsp.Content;
                        logger.LogInformation(rsp.Content?.ToJson());
                        logger.LogInformation(rsp.Content?.ValidationErrors?.ToJson());

                        
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        logger.LogError(rsp.Error, $"Error getting user permissions");

                        //logger.LogError(rsp.Error.Content, rsp.Error);

                        // foreach (var error in rsp.Content?.ValidationErrors)
                        // {
                        //     ModelState.AddModelError("", error.Error);
                        // }

                        return Page();
                    }
                }

                // If we got this far, something failed, redisplay form
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error accessing API and setting user claims");
                logger.LogError(ex, ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}