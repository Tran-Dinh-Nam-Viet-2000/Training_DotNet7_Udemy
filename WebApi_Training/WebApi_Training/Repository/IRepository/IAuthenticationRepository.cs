using WebApi_Training.Dto;
using WebApi_Training.Models;

namespace WebApi_Training.Repository.IRepository
{
    public interface IAuthenticationRepository
    {
        bool IsUnquine(string username);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequest);
        Task<User> Register(RegisterRequestDto registerRequest);
    }
}
