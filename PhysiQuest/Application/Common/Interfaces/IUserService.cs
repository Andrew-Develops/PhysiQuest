using Application.Badges.DTO;
using Application.UserBadges.DTO;
using Application.UserQuests.DTO;
using Application.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<UserDTO> CreateUserAsync(CreateAndUpdateDTO userDto);
        Task<UserDTO> UpdateUserAsync(int id, CreateAndUpdateDTO userDto);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<UserDTO>> GetUsersByPointsDescendingAsync();
        Task<UserBadgeDTO> AddBadgeToUserAsync(AssignBadgeDTO assignBadgeDto);
        Task<IEnumerable<BadgeDTO>> GetUserBadgesByNameAsync(string userName);
        Task<UserQuestDTO> AssignQuestToUserAsync(string username, int questId);

    }
}
