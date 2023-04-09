using Application.Badges.DTO;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IBadgeService
    {
        Task<IEnumerable<BadgeDTO>> GetAllBadgesAsync();
        Task<BadgeDTO> GetBadgeByIdAsync(int id);
        Task<BadgeDTO> CreateBadgeAsync(CreateAndUpdateBadgeDTO badgeDto);
        Task<BadgeDTO> UpdateBadgeAsync(int id, CreateAndUpdateBadgeDTO badgeDto);
        Task<bool> DeleteBadgeAsync(int id);
        Task<UserBadge> DeleteUserBadgeAsync(string username, int badgeId);


    }
}
