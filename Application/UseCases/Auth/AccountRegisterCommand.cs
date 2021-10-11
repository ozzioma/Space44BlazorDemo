using System.ComponentModel.DataAnnotations;
using Common;
using MediatR;

namespace Application.UseCases.Auth
{
    public class AccountRegisterCommand : IRequest<CommandResult<RegisterResponse>>
    {
        [Required] [MaxLength(20)] public string UserName { get; set; }

        [Required] [MaxLength(20)] public string Password { get; set; }
    }
}