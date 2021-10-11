using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.UseCases.Student
{
    public class StudentDeleteCommandValidator : AbstractValidator<StudentDeleteCommand>
    {
        private readonly StudentDbContext dbContext;
        private readonly ILogger<StudentDeleteCommandValidator> logger;

        public StudentDeleteCommandValidator(StudentDbContext appDbContext,
            ILogger<StudentDeleteCommandValidator> _logger)
        {
            dbContext = appDbContext;
            logger = _logger;

            RuleFor(p => p.Id).NotNull().GreaterThan(0);

            RuleFor(p => p).Custom((data, context) =>
            {
                var checkId = dbContext.Students.Where(r => r.Id == data.Id).Any();
                if (!checkId)
                {
                    context.AddFailure(
                        new ValidationFailure(nameof(data.Id),
                            "User with selected Id does not exist", data.Id));
                }
            });
        }
    }
}