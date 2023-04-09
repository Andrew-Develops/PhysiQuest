using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Quests.DTO;
using Application.UserQuests.DTO;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestController : ControllerBase
    {
        private readonly IQuestService _questService;
        private readonly IUserService _userService;

        public QuestController(IQuestService questService, IUserService userService)
        {
            _questService = questService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestDTO>>> GetAllQuestsAsync()
        {
            var quests = await _questService.GetAllQuestsAsync();
            return Ok(quests);
        }

        [HttpGet("{id}", Name = "GetQuestById")]
        public async Task<ActionResult<QuestDTO>> GetQuestByIdAsync(int id)
        {
            try
            {
                var quest = await _questService.GetQuestByIdAsync(id);
                return Ok(quest);
            }
            catch (QuestNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<QuestDTO>> CreateQuestAsync([FromBody] CreateAndUpdateQuestDTO questDto)
        {
            var newQuest = await _questService.CreateQuestAsync(questDto);
            return CreatedAtRoute("GetQuestById", new { id = newQuest.Id }, newQuest);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuestAsync(int id, [FromBody] CreateAndUpdateQuestDTO questDto)
        {
            try
            {
                var updatedQuest = await _questService.UpdateQuestAsync(id, questDto);
                return Ok(updatedQuest);
            }
            catch (QuestNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestAsync(int id)
        {
            try
            {
                var result = await _questService.DeleteQuestAsync(id);
                return NoContent();
            }
            catch (QuestNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("assign/{username}/{questId}")]
        public async Task<ActionResult<UserQuestDTO>> AssignQuestToUserAsync(string username, int questId)
        {
            try
            {
                var userQuest = await _userService.AssignQuestToUserAsync(username, questId);
                return Ok(userQuest);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (QuestNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("complete/{questId}")]
        public async Task<ActionResult<UserQuestDTO>> CompleteUserQuestAsync(int questId, [FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username is required");
            }

            var completedUserQuest = await _questService.CompleteUserQuestAsync(username, questId);

            if (completedUserQuest == null)
            {
                return NotFound();
            }

            return Ok(completedUserQuest);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<UserQuestDetailDTO>>> GetUserQuestsAsync([FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username is required");
            }

            var userQuests = await _questService.GetUserQuestsAsync(username);
            return Ok(userQuests);
        }

        [HttpDelete("delete/{questId}")]
        public async Task<ActionResult<UserQuestDTO>> DeleteUserQuestAsync(int questId, [FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username is required");
            }

            var deletedUserQuest = await _questService.DeleteUserQuestAsync(username, questId);

            if (deletedUserQuest == null)
            {
                return NotFound();
            }

            return Ok(deletedUserQuest);
        }

        [HttpGet("alphabetical")]
        public async Task<ActionResult<IEnumerable<QuestDTO>>> GetQuestsAlphabeticalAsync()
        {
            var quests = await _questService.GetQuestsAlphabeticalAsync();
            return Ok(quests);
        }

        [HttpGet("reward-points")]
        public async Task<ActionResult<IEnumerable<QuestDTO>>> GetQuestsByRewardPointsAsync()
        {
            var quests = await _questService.GetQuestsByRewardPointsAsync();
            return Ok(quests);
        }

        [HttpGet("reward-tokens")]
        public async Task<ActionResult<IEnumerable<QuestDTO>>> GetQuestsByRewardTokensAsync()
        {
            var quests = await _questService.GetQuestsByRewardTokensAsync();
            return Ok(quests);
        }

    }

}
