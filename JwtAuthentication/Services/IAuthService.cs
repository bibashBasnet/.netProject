using JwtAuthentication.DTO;
using JwtAuthentication.Model;

namespace JwtAuthentication.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<string?> LoginAsync(UserDto request);
        Task<IEnumerable<NormalDto>> GetUserAsync();
        Task<User?> GetUserByIdAsync(int Id);
        Task<NormalDto?> DeleteUserAsync(int Id);
        Task<NormalDto?> UpdateUserAsync(int Id, UserDto updateData);

    }

}
