using JwtAuthentication.DTO;
using JwtAuthentication.Model;
using JwtAuthentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IAuthService authService) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NormalDto>>> GetUser(
            int page = 1, int pageSize = 5, string word = "", string sortBy = "", string sortOrder = "asc"
            )
        {
            var userList = await authService.GetUserAsync();
            if (!string.IsNullOrWhiteSpace(word))
            {
                userList = userList.Where(x => x.UserName.StartsWith(word, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var totalCount = userList.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            var productPerPage = userList.Skip((page - 1) * pageSize).Take(pageSize);
            switch (sortBy.ToLower())
            {
                case "username":
                    productPerPage = sortOrder.ToLower() == "desc" ?
                        productPerPage.OrderByDescending(x => x.UserName).ToList() :
                        productPerPage.OrderBy(x => x.UserName).ToList();
                    break;
                case "id":
                    productPerPage = sortOrder.ToLower() == "desc" ?
                        productPerPage.OrderByDescending(x => x.Id).ToList() :
                        productPerPage.OrderBy(x => x.Id).ToList();
                    break;
                default:
                    productPerPage = sortOrder.ToLower() == "desc" ?
                        productPerPage.OrderByDescending(x => x.UserName).ToList() :
                        productPerPage.OrderBy(x => x.UserName).ToList();
                    break;
            }
            var users = productPerPage.Select(u => new NormalDto
            {
                Id = u.Id,
                UserName = u.UserName
            }).ToList();    
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id) {
            var user = await authService.GetUserByIdAsync(id);
            if(user is null)
            {
                return BadRequest("User does not exist");
            }
            return Ok(user);
        }

        [Authorize]
        [HttpDelete("{id}")]

        public async Task<ActionResult<NormalDto>> DeleteUser(int id)
        {
            var user = await authService.GetUserByIdAsync(id);
            if(user is null)
            {
                return BadRequest("User does not exist"); 
            }
            await authService.DeleteUserAsync(id);
            return Ok($"User has been deleted.");
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<NormalDto>> UpdateUser(int id, UserDto updateData)
        {
            var user = await authService.GetUserByIdAsync(id);
            if(user is null)
            {
                return BadRequest("The user does not exist");
            }
            var updatedData = await authService.UpdateUserAsync(id, updateData);
            if (updatedData is null) {
                return BadRequest();
            }
            return Ok(updatedData);
        }

    }
}
