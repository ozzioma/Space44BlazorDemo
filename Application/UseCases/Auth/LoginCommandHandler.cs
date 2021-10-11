using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace Application.UseCases.Auth
{
    public class LoginCommandHandler : IRequestHandler<UserLoginCommand, CommandResult<LoginResponse>>
    {
        private readonly ILogger<LoginCommandHandler> logger;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public LoginCommandHandler(
            ILogger<LoginCommandHandler> _logger,
            UserManager<AppUser> _userManager,
            RoleManager<IdentityRole> _roleManager,
            IConfiguration _configuration)
        {
            this.userManager = _userManager;
            this.roleManager = _roleManager;
            configuration = _configuration;
            logger = _logger;
        }

        public async Task<CommandResult<LoginResponse>> Handle(UserLoginCommand request,
            CancellationToken cancellationToken)
        {
            CommandResult<LoginResponse> rsp = new CommandResult<LoginResponse>();

            var user = await userManager.FindByNameAsync(request.UserName);

            if (user != null && await userManager.CheckPasswordAsync(user, request.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                rsp.Message = "Login successful";
                rsp.ErrorFlag = false;
                rsp.Response = new LoginResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    TokenExpirationDate = token.ValidTo
                };

                return rsp;
            }

            rsp.Message = "Invalid login details";
            rsp.ErrorFlag = true;
            return rsp;
        }
    }
}