using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.UseCases.Auth
{
    public class RegisterCommandHandler :
        IRequestHandler<AccountRegisterCommand, CommandResult<RegisterResponse>>
    {
        private readonly ILogger<RegisterCommandHandler> logger;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public RegisterCommandHandler(
            ILogger<RegisterCommandHandler> _logger,
            UserManager<AppUser> _userManager,
            RoleManager<IdentityRole> _roleManager,
            IConfiguration _configuration)
        {
            this.userManager = _userManager;
            this.roleManager = _roleManager;
            configuration = _configuration;
            logger = _logger;
        }

        public async Task<CommandResult<RegisterResponse>> Handle(AccountRegisterCommand request,
            CancellationToken cancellationToken)
        {
            CommandResult<RegisterResponse> rsp = new CommandResult<RegisterResponse>();

            var userExists = await userManager.FindByNameAsync(request.UserName);
            if (userExists != null)
            {
                rsp.Message = "User already exists";
                rsp.ErrorFlag = true;
            }

            AppUser user = new AppUser()
            {
                Email = request.UserName,
                UserName = request.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                rsp.Message = result.Errors?.ToList()?[0]?.Description;
                rsp.ErrorFlag = true;
                return rsp;
            }

            rsp.Message = "User created";
            rsp.ErrorFlag = false;
            return rsp;
        }
    }
}