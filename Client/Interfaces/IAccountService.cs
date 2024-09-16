
using Shared.Models;
using static Shared.Models.ServiceResponses;

namespace Client.Interfaces;

public interface IAccountService
{
    Task<LoginResponse> LoginAsync(LoginDTO loginDTO);
    Task LogoutAsync();
}


