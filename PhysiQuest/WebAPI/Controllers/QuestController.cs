using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Quests.DTO;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestController : ControllerBase
    {
        private readonly IQuestService _questService;

        public QuestController(IQuestService questService)
        {
            _questService = questService;
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
    }

}
