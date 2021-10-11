using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.UseCases.Student
{
    public class StudentQueryCommandHandler : IRequestHandler<StudentQueryCommand,
        CommandResult<IQueryable<Persistence.Student>>>
    {
        private readonly ILogger<StudentQueryCommandHandler> logger;
        private readonly StudentDbContext dbContext;
        private readonly IConfiguration configuration;

        public StudentQueryCommandHandler(
            ILogger<StudentQueryCommandHandler> _logger,
            StudentDbContext _dbContext,
            IConfiguration _configuration)
        {
            dbContext = _dbContext;
            configuration = _configuration;
            logger = _logger;
        }

        public Task<CommandResult<IQueryable<Persistence.Student>>> Handle(StudentQueryCommand request,
            CancellationToken cancellationToken)
        {
            var rsp = new CommandResult<IQueryable<Persistence.Student>>();
            rsp.Response = dbContext.Students;

            return Task.FromResult(rsp);
        }
    }
}