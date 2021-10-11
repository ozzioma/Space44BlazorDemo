using System.Threading.Tasks;
using Application.UseCases.Auth;
using Common;
using Refit;

namespace Application.Infrastructure
{
    public interface AuthenticationService
    {
        [Post("/Account/login")]
        Task<ApiResponse<CommandResult<LoginResponse>>> Login([Body] UserLoginCommand command);

        [Post("/Account/register")]
        Task<ApiResponse<CommandResult<RegisterResponse>>> Register([Body] AccountRegisterCommand command);
    }
}