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
    public class StudentCreateCommandHandler : IRequestHandler<StudentCreateCommand,
        CommandResult<StudentViewModel>>
    {
        private readonly ILogger<StudentCreateCommandHandler> logger;
        private readonly StudentDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;


        public StudentCreateCommandHandler(
            ILogger<StudentCreateCommandHandler> _logger,
            StudentDbContext _dbContext,
            IMapper _mapper,
            IConfiguration _configuration)
        {
            dbContext = _dbContext;
            configuration = _configuration;
            logger = _logger;
            mapper = _mapper;
        }

        public async Task<CommandResult<StudentViewModel>> Handle(StudentCreateCommand request,
            CancellationToken cancellationToken)
        {
            var rsp = new CommandResult<StudentViewModel>();
            var entity = mapper.Map<Persistence.Student>(request);

            dbContext.Students.Add(entity);
            await dbContext.SaveChangesAsync();

            rsp.Response = mapper.Map<StudentViewModel>(entity);

            return rsp;
        }
    }
}