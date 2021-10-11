using System.ComponentModel.DataAnnotations;
using Common;
using MediatR;

namespace Application.UseCases.Student
{
    public class UserLoginCommand: IRequest<CommandResult<string>>
    {
        [Required] [MaxLength(20)] 
        public string UserName { get; set; }
        
        [Required] [MaxLength(20)] 
        public string Password { get; set; }
    }
}