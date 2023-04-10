using Application.Quests.DTO;
using Application.UserQuests.DTO;

namespace Application.Common.Interfaces
{
    public interface IQuestService
    {
        Task<IEnumerable<QuestDTO>> GetAllQuestsAsync();
        Task<QuestDTO> GetQuestByIdAsync(int id);
        Task<QuestDTO> CreateQuestAsync(CreateAndUpdateQuestDTO questDto);
        Task<QuestDTO> UpdateQuestAsync(int id, CreateAndUpdateQuestDTO questDto);
        Task<bool> DeleteQuestAsync(int id);
        Task<UserQuestDTO> CompleteUserQuestAsync(string username, int questId, string imageUrl);
        Task<IEnumerable<UserQuestDetailWithIdDTO>> GetUserQuestsAsync(string username);

        Task<UserQuestDTO> DeleteUserQuestAsync(string username, int questId);
        Task<IEnumerable<QuestDTO>> GetQuestsAlphabeticalAsync();
        Task<IEnumerable<QuestDTO>> GetQuestsByRewardPointsAsync();
        Task<IEnumerable<QuestDTO>> GetQuestsByRewardTokensAsync();
    }
}
