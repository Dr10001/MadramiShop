using MadramiShop.Shared.Dtos;
using Refit;

namespace MadramiShop.Apis
{
    public interface IAuthApi
    {
        [Post("/api/auth/register")]
        Task<ApiResult> RegisterAsync(RegisterDto dto);
        [Post("/api/auth/login")]
        Task<ApiResult<LoggedInUser>> LoginAsync(LoginDto dto);
    }


}
