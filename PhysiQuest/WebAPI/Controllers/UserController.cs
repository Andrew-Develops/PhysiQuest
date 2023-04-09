using Application.Badges.DTO;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Quests.DTO;
using Application.UserBadges.DTO;
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

        /// <summary>
        /// Retrieves all the users.
        /// </summary>
        /// <returns>A list of all the users.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user with the specified ID.</returns>
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

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userDto">The data for the new user.</param>
        /// <returns>The newly created user.</returns>
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

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="userDto">The updated data for the user.</param>
        /// <returns>The updated user.</returns>
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

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>A response indicating the success of the operation.</returns>
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

        /// <summary>
        /// Retrieves the top users by their points in descending order.
        /// </summary>
        /// <returns>A list of the top users by points.</returns>
        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersByPointsDescendingAsync()
        {
            var users = await _userService.GetUsersByPointsDescendingAsync();
            return Ok(users);
        }

        /// <summary>
        /// Assigns a badge to a user.
        /// </summary>
        /// <param name="userId">The ID of the user to assign the badge to.</param>
        /// <param name="assignBadgeDto">The data transfer object containing the ID of the badge to assign.</param>
        /// <returns>The newly assigned user badge.</returns>
        [HttpPost("{userId}/badges")]
        public async Task<ActionResult<UserBadgeDTO>> AddBadgeToUserAsync(int userId, [FromBody] AssignBadgeDTO assignBadgeDto)
        {
            assignBadgeDto.UserId = userId;

            try
            {
                var userBadge = await _userService.AddBadgeToUserAsync(assignBadgeDto);
                return Ok(userBadge);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadgeNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Gets the badges assigned to a user by their name.
        /// </summary>
        /// <param name="userName">The name of the user to get the badges for.</param>
        /// <returns>A collection of the user's badges.</returns>
        [HttpGet("{userName}/badges")]
        public async Task<ActionResult<IEnumerable<BadgeDTO>>> GetUserBadgesByNameAsync(string userName)
        {
            try
            {
                var badges = await _userService.GetUserBadgesByNameAsync(userName);
                return Ok(badges);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("create-user-quest")]
        public async Task<ActionResult<QuestDTO>> CreateUserQuestAsync([FromQuery] string username, [FromBody] CreateAndUpdateQuestDTO questDto)
        {
            try
            {
                var userQuest = await _userService.CreateUserQuestAsync(username, questDto);
                return Ok(userQuest);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InsufficientTokensException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicateQuestException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
