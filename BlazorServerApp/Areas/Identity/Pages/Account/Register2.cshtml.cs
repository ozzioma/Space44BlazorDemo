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
    public class Register2Model : PageModel
    {
        private AuthenticationService authenticationService;
        private readonly ILogger<Register2Model> logger;

        public Register2Model(ILogger<Register2Model> _logger,
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
            [StringLength(maximumLength: 20, MinimumLength = 6,
                ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string PasswordConfirm { get; set; }
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
                    AccountRegisterCommand request = new AccountRegisterCommand
                    {
                        UserName = Input.Email,
                        Password = Input.Password,
                    };

                    var rsp = await authenticationService.Register(request);

                    if (rsp.IsSuccessStatusCode)
                    {
                        logger.LogInformation("User register.");
                        return LocalRedirect("~/Identity/Account/Login2");
                    }
                    else
                    {
                        var data = rsp.Content;
                        logger.LogInformation(rsp.Content?.ToJson());
                        logger.LogInformation(rsp.Content?.ValidationErrors?.ToJson());


                        ModelState.AddModelError(string.Empty, "Error registering user.");
                        logger.LogError(rsp.Error, $"Error registering user");

                        //logger.LogError(rsp.Error.Content, rsp.Error);

                        // foreach (var error in rsp.Content?.ValidationErrors)
                        // {
                        //     ModelState.AddModelError("", error.Error);
                        // }

                        return Page();
                    }
                }

                // If we got this far, something failed, redisplay form
                ModelState.AddModelError(string.Empty, "Error registering user.");
                return Page();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error accessing API and registering user");
                logger.LogError(ex, ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}