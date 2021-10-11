using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.UseCases.Student
{
    public class StudentDeleteCommandHandler : IRequestHandler<StudentDeleteCommand,
        CommandResult<string>>
    {
        private readonly ILogger<StudentDeleteCommandHandler> logger;
        private readonly StudentDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;


        public StudentDeleteCommandHandler(
            ILogger<StudentDeleteCommandHandler> _logger,
            StudentDbContext _dbContext,
            IMapper _mapper,
            IConfiguration _configuration)
        {
            dbContext = _dbContext;
            configuration = _configuration;
            logger = _logger;
            mapper = _mapper;
        }

        public async Task<CommandResult<string>> Handle(StudentDeleteCommand request,
            CancellationToken cancellationToken)
        {
            var rsp = new CommandResult<string>();
            var entity = await dbContext.Students.FindAsync(request.Id);

            dbContext.Students.Remove(entity);
            await dbContext.SaveChangesAsync();

            rsp.Response = "Student successfully deleted";

            return rsp;
        }
    }
}