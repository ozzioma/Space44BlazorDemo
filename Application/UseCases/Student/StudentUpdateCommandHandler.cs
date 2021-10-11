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
    public class StudentUpdateCommandHandler : IRequestHandler<StudentUpdateCommand,
        CommandResult<StudentViewModel>>
    {
        private readonly ILogger<StudentUpdateCommandHandler> logger;
        private readonly StudentDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;


        public StudentUpdateCommandHandler(
            ILogger<StudentUpdateCommandHandler> _logger,
            StudentDbContext _dbContext,
            IMapper _mapper,
            IConfiguration _configuration)
        {
            dbContext = _dbContext;
            configuration = _configuration;
            logger = _logger;
            mapper = _mapper;
        }

        public async Task<CommandResult<StudentViewModel>> Handle(StudentUpdateCommand request,
            CancellationToken cancellationToken)
        {
            var rsp = new CommandResult<StudentViewModel>();
            var entity = await dbContext.Students.FindAsync(request.Id);

            mapper.Map(request, entity);

            dbContext.Students.Update(entity);
            await dbContext.SaveChangesAsync();

            rsp.Response = mapper.Map<StudentViewModel>(entity);

            return rsp;
        }
    }
}