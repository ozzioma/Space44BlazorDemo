using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.UseCases.Auth
{
    public class RegisterCommandValidator : AbstractValidator<AccountRegisterCommand>
    {
        private readonly StudentDbContext dbContext;
        private readonly ILogger<RegisterCommandValidator> logger;

        public RegisterCommandValidator(StudentDbContext appDbContext,
            ILogger<RegisterCommandValidator> _logger)
        {
            dbContext = appDbContext;
            logger = _logger;

            RuleFor(p => p.UserName).NotNull().MaximumLength(20);
            RuleFor(p => p.Password).NotNull().MinimumLength(6).MaximumLength(20);

            RuleFor(p => p).Custom((data, context) =>
            {
                var checkUserName = dbContext.Users.Where(r => r.UserName == data.UserName).Any();
                if (checkUserName)
                {
                    context.AddFailure(
                        new ValidationFailure(nameof(data.UserName),
                            "User name already exists", data.UserName));
                }
            });
        }
    }
}