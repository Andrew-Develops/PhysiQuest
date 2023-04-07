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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BadgeDTO>>> GetAllBadgesAsync()
        {
            var badges = await _badgeService.GetAllBadgesAsync();
            return Ok(badges);
        }

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

        [HttpPost]
        public async Task<ActionResult<BadgeDTO>> CreateBadgeAsync([FromBody] CreateAndUpdateBadgeDTO badgeDto)
        {
            var newBadge = await _badgeService.CreateBadgeAsync(badgeDto);
            return CreatedAtRoute("GetBadgeById", new { id = newBadge.Id }, newBadge);
        }

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
    }

}
