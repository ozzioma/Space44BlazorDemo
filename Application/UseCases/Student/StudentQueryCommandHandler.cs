using System.Linq;
using Common;
using MediatR;

namespace Application.UseCases.Student
{
    public class StudentQueryCommandHandler : IRequestHandler<StudentQueryCommand, CommandResult<IQueryable<Persistence.Student>>>
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
    }
}