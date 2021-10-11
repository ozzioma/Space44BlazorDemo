using System.Threading.Tasks;
using Refit;

namespace Application.Infrastructure
{
    public interface IEntityDataService
    {
        //C,U,D=>Command objects for Cread, Update & Delete
        //V=>ViewModel for response type

        [Post("/{entity}/create")]
        Task<ApiResponse<V>> Create<C, V>(string entity, [Body] C createPayload);

        [Post("/{entity}/update")]
        Task<ApiResponse<V>> Update<U, V>(string entity, [Body] U updatePayload);

        [Post("/{entity}/delete")]
        Task<ApiResponse<V>> Delete<D, V>(string entity, [Body] D deletePayload);

        [Post("/{entity}/process/{command}")]
        Task<ApiResponse<V>> Process<P, V>(string entity, string command, [Body] P processPayload);
    }


    public class WebConfigHelper
    {
    }
}