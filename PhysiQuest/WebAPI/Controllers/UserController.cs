using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Users.DTO;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<ActionResult<UserDTO>> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUserAsync([FromBody] CreateAndUpdateDTO userDto)
        {
            try
            {
                var newUser = await _userService.CreateUserAsync(userDto);
                return CreatedAtRoute("GetUserById", new { id = newUser.Id }, newUser);
            }
            catch (DuplicateEmailException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUserAsync(int id, [FromBody] CreateAndUpdateDTO userDto)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(id, userDto);
                return Ok(updatedUser);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DuplicateEmailException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersByPointsDescendingAsync()
        {
            var users = await _userService.GetUsersByPointsDescendingAsync();
            return Ok(users);
        }
    }
}
