using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Quests.DTO;
using Application.UserQuests.DTO;
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

        /// <summary>
        /// Retrieves all quests from the database and returns them as a collection of QuestDTO objects.
        /// </summary>
        /// <returns>A collection of QuestDTO objects representing all the quests in the database.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestDTO>>> GetAllQuestsAsync()
        {
            var quests = await _questService.GetAllQuestsAsync();
            return Ok(quests);
        }

        /// <summary>
        /// Retrieves a specific quest from the database by its ID and returns it as a QuestDTO object.
        /// </summary>
        /// <param name="id">The ID of the quest to retrieve.</param>
        /// <returns>A QuestDTO object representing the retrieved quest.</returns>
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

        /// <summary>
        /// Creates a new quest in the database based on the provided data and returns it as a QuestDTO object.
        /// </summary>
        /// <param name="questDto">The data of the quest to create.</param>
        /// <returns>A QuestDTO object representing the newly created quest.</returns>
        [HttpPost]
        public async Task<ActionResult<QuestDTO>> CreateQuestAsync([FromBody] CreateAndUpdateQuestDTO questDto)
        {
            var newQuest = await _questService.CreateQuestAsync(questDto);
            return CreatedAtRoute("GetQuestById", new { id = newQuest.Id }, newQuest);
        }

        /// <summary>
        /// Updates an existing quest in the database based on the provided ID and data and returns it as a QuestDTO object.
        /// </summary>
        /// <param name="id">The ID of the quest to update.</param>
        /// <param name="questDto">The updated data of the quest.</param>
        /// <returns>A QuestDTO object representing the updated quest.</returns>
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

        /// <summary>
        /// Deletes a specific quest from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the quest to delete.</param>
        /// <returns>A NoContentResult indicating a successful deletion.</returns>
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

        /// <summary>
        /// Assigns a specific quest to a specific user in the database based on their usernames and quest ID and returns it as a UserQuestDTO object.
        /// </summary>
        /// <param name="username">The username of the user to assign the quest to.</param>
        /// <param name="questId">The ID of the quest to assign.</param>
        /// <returns>A UserQuestDTO object representing the assigned user quest.</returns>
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

        /// <summary>
        /// Completes a user quest for the given quest id and username.
        /// </summary>
        /// <param name="questId">The quest id.</param>
        /// <param name="username">The username.</param>
        /// <returns>Returns an ActionResult with the completed UserQuestDTO.</returns>
        [HttpPut("complete/{questId}")]
        public async Task<ActionResult<UserQuestDTO>> CompleteUserQuestAsync(int questId, [FromQuery] string username, [FromForm] string imageUrl)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username is required");
            }

            var completedUserQuest = await _questService.CompleteUserQuestAsync(username, questId, imageUrl);

            if (completedUserQuest == null)
            {
                return NotFound();
            }

            return Ok(completedUserQuest);
        }

        /// <summary>
        /// Gets the user quests for the given username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Returns an ActionResult with the IEnumerable of UserQuestDetailDTO.</returns>
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

        /// <summary>
        /// Deletes a user quest for the given quest id and username.
        /// </summary>
        /// <param name="questId">The quest id.</param>
        /// <param name="username">The username.</param>
        /// <returns>Returns an ActionResult with the deleted UserQuestDTO.</returns>
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

        /// <summary>
        /// Gets the quests in alphabetical order.
        /// </summary>
        /// <returns>Returns an ActionResult with the IEnumerable of QuestDTO.</returns>
        [HttpGet("alphabetical")]
        public async Task<ActionResult<IEnumerable<QuestDTO>>> GetQuestsAlphabeticalAsync()
        {
            var quests = await _questService.GetQuestsAlphabeticalAsync();
            return Ok(quests);
        }

        /// <summary>
        /// Gets the quests sorted by reward points.
        /// </summary>
        /// <returns>Returns an ActionResult with the IEnumerable of QuestDTO.</returns>
        [HttpGet("reward-points")]
        public async Task<ActionResult<IEnumerable<QuestDTO>>> GetQuestsByRewardPointsAsync()
        {
            var quests = await _questService.GetQuestsByRewardPointsAsync();
            return Ok(quests);
        }

        /// <summary>
        /// Gets the quests sorted by reward tokens.
        /// </summary>
        /// <returns>Returns an ActionResult with the IEnumerable of QuestDTO.</returns>
        [HttpGet("reward-tokens")]
        public async Task<ActionResult<IEnumerable<QuestDTO>>> GetQuestsByRewardTokensAsync()
        {
            var quests = await _questService.GetQuestsByRewardTokensAsync();
            return Ok(quests);
        }
    }
}
