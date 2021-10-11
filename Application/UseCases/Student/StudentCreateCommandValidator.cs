using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.UseCases.Student
{
    public class StudentCreateCommandValidator : AbstractValidator<StudentCreateCommand>
    {
        private readonly StudentDbContext dbContext;
        private readonly ILogger<StudentCreateCommandValidator> logger;

        public StudentCreateCommandValidator(StudentDbContext appDbContext,
            ILogger<StudentCreateCommandValidator> _logger)
        {
            dbContext = appDbContext;
            logger = _logger;

            RuleFor(p => p.UserName).NotEmpty().MaximumLength(20);
            RuleFor(p => p.FirstName).NotEmpty().MaximumLength(20);
            RuleFor(p => p.LastName).NotEmpty().MaximumLength(20);
            RuleFor(p => p.Career).NotEmpty().MaximumLength(50);
            RuleFor(p => p.Age).NotNull().LessThanOrEqualTo(70).GreaterThan(0);


            RuleFor(p => p).Custom((data, context) =>
            {
                var checkUserName = dbContext.Students.Where(r => r.UserName == data.UserName).Any();
                if (checkUserName)
                {
                    context.AddFailure(
                        new ValidationFailure(nameof(data.UserName),
                            "User with name already exists", data.UserName));
                }
            });
        }
    }
}