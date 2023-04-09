using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUserQuestRepository
    {
        Task<IEnumerable<UserQuest>> GetUserQuestsAsync();
        Task<UserQuest> GetUserQuestByIdAsync(int id);
        Task<UserQuest> CreateUserQuestAsync(UserQuest userQuest);
        Task<UserQuest> UpdateUserQuestAsync(UserQuest userQuest);
        Task<bool> DeleteUserQuestAsync(int id);
        Task<UserQuest> CompleteUserQuestAsync(string username, int questId);
        Task<IEnumerable<UserQuest>> GetUserQuestsAsync(string username);
        Task<UserQuest> DeleteUserQuestAsync(string username, int questId);


    }

}
