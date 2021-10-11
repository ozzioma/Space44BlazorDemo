using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.UseCases.Student
{
    public class StudentUpdateCommandValidator : AbstractValidator<StudentUpdateCommand>
    {
        private readonly StudentDbContext dbContext;
        private readonly ILogger<StudentUpdateCommandValidator> logger;

        public StudentUpdateCommandValidator(StudentDbContext appDbContext,
            ILogger<StudentUpdateCommandValidator> _logger)
        {
            dbContext = appDbContext;
            logger = _logger;

            RuleFor(p => p.Id).NotNull().GreaterThan(0);
            RuleFor(p => p.UserName).NotEmpty().MaximumLength(20);
            RuleFor(p => p.FirstName).NotEmpty().MaximumLength(20);
            RuleFor(p => p.LastName).NotEmpty().MaximumLength(20);
            RuleFor(p => p.Career).NotEmpty().MaximumLength(50);
            RuleFor(p => p.Age).NotNull().LessThanOrEqualTo(70).GreaterThan(0);


            RuleFor(p => p).Custom((data, context) =>
            {
                var checkId = dbContext.Students.Where(r => r.Id == data.Id).Any();
                if (!checkId)
                {
                    context.AddFailure(
                        new ValidationFailure(nameof(data.Id),
                            "User with selected Id does not exist", data.Id));
                }

                var checkUserName = dbContext.Students.Where(r => r.UserName == data.UserName
                                                                  && r.Id != data.Id).Any();
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