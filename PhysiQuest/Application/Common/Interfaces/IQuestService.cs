using Application.Quests.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IQuestService
    {
        Task<IEnumerable<QuestDTO>> GetAllQuestsAsync();
        Task<QuestDTO> GetQuestByIdAsync(int id);
        Task<QuestDTO> CreateQuestAsync(CreateAndUpdateQuestDTO questDto);
        Task<QuestDTO> UpdateQuestAsync(int id, CreateAndUpdateQuestDTO questDto);
        Task<bool> DeleteQuestAsync(int id);
    }
}
