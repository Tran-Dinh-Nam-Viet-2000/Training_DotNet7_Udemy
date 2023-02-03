using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi_Training.Database;
using WebApi_Training.Dto;
using WebApi_Training.Models;
using WebApi_Training.Repository.IRepository;

namespace WebApi_Training.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private string secretKey;

        public AuthenticationRepository(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            secretKey = _configuration.GetValue<string>("ApiSettings:Secret");
        }
        public bool IsUnquine(string username)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
                return true;
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
        {
            //check account nhập vào có tồn tại không
            var user = await _dbContext.Users.FirstOrDefaultAsync(n => n.UserName== loginRequest.UserName
                                                            && n.Password == loginRequest.Password);
            if (user == null) 
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            //Tạo nội dung cho token tự gen ra, token thực chất là thông tin của User
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                //Nơi thêm nội dung vào token
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role),
                }),
                //Set hiệu lực token hết trong bao lâu
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //Tạo token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDto loginResponse = new LoginResponseDto()
            {
                UserName = user.UserName,
                //Set token
                Token = new JwtSecurityTokenHandler().WriteToken(token),
            };

            return loginResponse;
        }

        public async Task<User> Register(RegisterRequestDto registerRequest)
        {
            registerRequest.UserName = registerRequest.UserName.Trim();
            User user = new()
            {
                Name = registerRequest.Name,
                UserName = registerRequest.UserName,
                Password = registerRequest.Password,
                Role = "Admin",
            };
            await _dbContext.AddAsync(user);
            _dbContext.SaveChanges();
            return user;
        }
    }
}
