using System.ComponentModel.DataAnnotations;
using Common;
using MediatR;

namespace Application.UseCases.Student
{
    public class StudentUpdateCommand : IRequest<CommandResult<StudentViewModel>>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [Range(minimum: 15, maximum: 50)]
        public int Age { get; set; }

        [Required]
        [MaxLength(50)]
        public string Career { get; set; }
    }
}