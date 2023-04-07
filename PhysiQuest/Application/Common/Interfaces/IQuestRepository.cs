using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

}
