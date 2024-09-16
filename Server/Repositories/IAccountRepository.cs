

using Shared.Models;
using static Shared.Models.ServiceResponses;


public interface IAccountRepository
{
    Task<LoginResponse> LoginAccount(LoginDTO loginDTO);
}