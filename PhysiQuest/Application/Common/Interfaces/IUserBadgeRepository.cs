using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUserBadgeRepository
    {
        Task<IEnumerable<UserBadge>> GetUserBadgesAsync();
        Task<UserBadge> GetUserBadgeByIdAsync(int userId, int badgeId);
        Task<UserBadge> CreateUserBadgeAsync(UserBadge userBadge);
        Task<UserBadge> UpdateUserBadgeAsync(UserBadge userBadge);
        Task<bool> DeleteUserBadgeAsync(int userId, int badgeId);
        Task<IEnumerable<UserBadge>> GetUserBadgesByUserIdAsync(int userId);
        Task<UserBadge> DeleteUserBadgeAsync(string username, int badgeId);

    }

}
