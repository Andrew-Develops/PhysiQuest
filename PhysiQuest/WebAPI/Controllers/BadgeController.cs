using Application.Badges.DTO;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BadgeController : ControllerBase
    {
        private readonly IBadgeService _badgeService;

        public BadgeController(IBadgeService badgeService)
        {
            _badgeService = badgeService;
        }

        /// <summary>
        /// Returns all badges.
        /// </summary>
        /// <returns>A list of BadgeDTOs</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BadgeDTO>>> GetAllBadgesAsync()
        {
            var badges = await _badgeService.GetAllBadgesAsync();
            return Ok(badges);
        }

        /// <summary>
        /// Returns a badge with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the badge to retrieve.</param>
        /// <returns>The BadgeDTO with the specified ID.</returns>
        [HttpGet("{id}", Name = "GetBadgeById")]
        public async Task<ActionResult<BadgeDTO>> GetBadgeByIdAsync(int id)
        {
            try
            {
                var badge = await _badgeService.GetBadgeByIdAsync(id);
                return Ok(badge);
            }
            catch (BadgeNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new badge.
        /// </summary>
        /// <param name="badgeDto">The data to create the badge from.</param>
        /// <returns>The created BadgeDTO.</returns>
        [HttpPost]
        public async Task<ActionResult<BadgeDTO>> CreateBadgeAsync([FromBody] CreateAndUpdateBadgeDTO badgeDto)
        {
            var newBadge = await _badgeService.CreateBadgeAsync(badgeDto);
            return CreatedAtRoute("GetBadgeById", new { id = newBadge.Id }, newBadge);
        }

        /// <summary>
        /// Updates an existing badge.
        /// </summary>
        /// <param name="id">The ID of the badge to update.</param>
        /// <param name="badgeDto">The new data for the badge.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBadgeAsync(int id, [FromBody] CreateAndUpdateBadgeDTO badgeDto)
        {
            try
            {
                var updatedBadge = await _badgeService.UpdateBadgeAsync(id, badgeDto);
                return Ok(updatedBadge);
            }
            catch (BadgeNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a badge with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the badge to delete.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBadgeAsync(int id)
        {
            try
            {
                var result = await _badgeService.DeleteBadgeAsync(id);
                return NoContent();
            }
            catch (BadgeNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a user's badge with the specified ID.
        /// </summary>
        /// <param name="username">The username of the user whose badge is to be deleted.</param>
        /// <param name="badgeId">The ID of the badge to delete.</param>
        /// <returns>An IActionResult indicating success or failure.</returns>
        [HttpDelete("user/{badgeId}")]
        public async Task<IActionResult> DeleteUserBadgeAsync(string username, int badgeId)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username is required");
            }

            var deletedUserBadge = await _badgeService.DeleteUserBadgeAsync(username, badgeId);

            if (deletedUserBadge == null)
            {
                return NotFound();
            }

            return Ok(deletedUserBadge);
        }
    }
}
