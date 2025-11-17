using JwtAuthentication.Data;
using JwtAuthentication.DTO;
using JwtAuthentication.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthentication.Services
{
    public class AuthService(UserContext _context, IConfiguration configuration) : IAuthService
    {

        private string CreateToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        public async Task<string?> LoginAsync(UserDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            if (user is null)
            {
                return null;
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return CreateToken(user);
        }

        public async Task<User?> RegisterAsync(UserDto request)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == request.UserName)) {
                return null;
            }
            var user = new User();

            var hashPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);
            user.UserName = request.UserName;
            user.Password = hashPassword;
            user.Role = request.Role;
            user.Age = request.Age;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task<IEnumerable<NormalDto>> GetUserAsync() { 
            var userList = await _context.Users.ToListAsync();
            var response = userList.Select(u => new NormalDto
            {
                Id = u.Id,
                UserName = u.UserName,
            });
            return response; 
        }


        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }

        public async Task<NormalDto?> DeleteUserAsync(int id) {
            var user = await _context.Users.FindAsync(id);
            if(user is null)
            {
                return null;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            var deletedUser = new NormalDto
            {
                Id = id,
                UserName = user.UserName,
            };
            return deletedUser;
        }

        public async Task<NormalDto?> UpdateUserAsync(int id, UserDto updateData) {
            var user = await _context.Users.FindAsync(id);
            if (user is null) { return null; }
            User usr = new User();
            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(usr, updateData.Password);

            user.UserName = updateData.UserName;
            user.Password = hashedPassword;
            user.Role = updateData.Role;
            user.Age = updateData.Age;
            await _context.SaveChangesAsync();
            NormalDto updatedUser = new NormalDto
            {
                Id = id,
                UserName = updateData.UserName,
                Role = updateData.Role,
                Age = updateData.Age
            };
            return updatedUser;
        }

    }
}
