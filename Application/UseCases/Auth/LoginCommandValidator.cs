using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.UseCases.Student
{
    public class LoginCommandValidator : AbstractValidator<UserLoginCommand>
    {
        private readonly StudentDbContext dbContext;
        private readonly ILogger<LoginCommandValidator> logger;

        public LoginCommandValidator(StudentDbContext appDbContext,
            ILogger<LoginCommandValidator> _logger)
        {
            dbContext = appDbContext;
            logger = _logger;

            RuleFor(p => p.UserName).NotNull().MaximumLength(20);


            RuleFor(p => p).Custom((data, context) =>
            {
                var checkUserName = dbContext.Users.Where(r => r.UserName == data.UserName).Any();
                if (!checkUserName)
                {
                    context.AddFailure(
                        new ValidationFailure(nameof(data.UserName),
                            "User does not exist", data.UserName));
                }
            });
        }
    }
}