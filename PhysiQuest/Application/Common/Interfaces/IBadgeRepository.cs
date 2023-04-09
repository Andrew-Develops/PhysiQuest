using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IBadgeRepository
    {
        Task<IEnumerable<Badge>> GetBadgesAsync();
        Task<Badge> GetBadgeByIdAsync(int id);
        Task<Badge> CreateBadgeAsync(Badge badge);
        Task<Badge> UpdateBadgeAsync(Badge badge);
        Task<bool> DeleteBadgeAsync(int id);
        Task<Badge> GetBadgeByNameAsync(string name);
        Task<IEnumerable<Badge>> GetBadgesByIdsAsync(IEnumerable<int> badgeIds);

    }

}
