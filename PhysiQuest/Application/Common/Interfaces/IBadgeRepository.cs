using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IBadgeRepository
    {
        Task<IEnumerable<Badge>> GetBadgesAsync();
        Task<Badge> GetBadgeByIdAsync(int id);
        Task<Badge> CreateBadgeAsync(Badge badge);
        Task<Badge> UpdateBadgeAsync(Badge badge);
        Task<bool> DeleteBadgeAsync(int id);
    }

}
