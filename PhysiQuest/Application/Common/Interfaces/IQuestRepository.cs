using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IQuestRepository
    {
        Task<IEnumerable<Quest>> GetQuestsAsync();
        Task<Quest> GetQuestByIdAsync(int id);
        Task<Quest> CreateQuestAsync(Quest quest);
        Task<Quest> UpdateQuestAsync(Quest quest);
        Task<bool> DeleteQuestAsync(int id);
        Task<Quest> GetQuestByTitleAsync(string name);
        Task<List<Quest>> GetQuestsAlphabeticalAsync();
        Task<List<Quest>> GetQuestsByRewardPointsAsync();
        Task<List<Quest>> GetQuestsByRewardTokensAsync();
    }
}
