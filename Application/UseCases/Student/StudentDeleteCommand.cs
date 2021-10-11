using System.ComponentModel.DataAnnotations;
using Common;
using MediatR;

namespace Application.UseCases.Student
{
    public class StudentDeleteCommand: IRequest<CommandResult<string>>
    {
        [Required]
        public int Id { get; set; }
    }
}