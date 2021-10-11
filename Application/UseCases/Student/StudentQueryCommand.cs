using System.Linq;
using Common;
using MediatR;

namespace Application.UseCases.Student
{
    public class StudentQueryCommand : IRequest<CommandResult<IQueryable<Persistence.Student>>>
    {
    }
}