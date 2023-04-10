using Application.Badges.DTO;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.UserBadges.DTO;
using Application.UserQuests.DTO;
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

        /// <summary>
        /// Gets the proof image URL for a given user and quest.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="questId">The ID of the quest.</param>
        /// <returns>An ActionResult containing the proof image URL if found, otherwise a NotFoundResult.</returns>
        [HttpGet("user/{username}/user-quest/{questId}/proof-image-url")]
        public async Task<ActionResult<string>> GetProofImageUrlAsync(string username, int questId)
        {
            try
            {
                var proofImageUrl = await _userService.GetProofImageUrlAsync(username, questId);

                if (string.IsNullOrEmpty(proofImageUrl))
                {
                    return NotFound("Proof image URL not found.");
                }

                return Ok(proofImageUrl);
            }
            catch (UserQuestNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the proof image URL for a given user and quest.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="questId">The ID of the quest.</param>
        /// <returns>An ActionResult containing the deleted user quest DTO if successful, otherwise a BadRequestResult or NotFoundResult.</returns>
        [HttpDelete("user/{username}/user-quest/{questId}/delete-proof-image-url")]
        public async Task<ActionResult<UserQuestDTO>> DeleteProofImageUrlAsync(string username, int questId)
        {
            try
            {
                var deletedProofUserQuestDto = await _userService.DeleteProofImageUrlAsync(username, questId);
                return Ok(deletedProofUserQuestDto);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UserQuestNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
