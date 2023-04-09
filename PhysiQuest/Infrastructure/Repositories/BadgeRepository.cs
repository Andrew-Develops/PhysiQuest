using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BadgeRepository : IBadgeRepository
    {
        private readonly ApplicationDbContext _context;

        public BadgeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Badge>> GetBadgesAsync()
        {
            return await _context.Badges.ToListAsync();
        }

        public async Task<Badge> GetBadgeByIdAsync(int id)
        {
            return await _context.Badges.FindAsync(id);
        }

        public async Task<Badge> CreateBadgeAsync(Badge badge)
        {
            _context.Badges.Add(badge);
            await _context.SaveChangesAsync();
            return badge;
        }

        public async Task<Badge> UpdateBadgeAsync(Badge badge)
        {
            _context.Entry(badge).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return badge;
        }

        public async Task<bool> DeleteBadgeAsync(int id)
        {
            var badge = await _context.Badges.FindAsync(id);
            if (badge == null)
            {
                return false;
            }

            _context.Badges.Remove(badge);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Badge> GetBadgeByNameAsync(string name)
        {
            return await _context.Badges.FirstOrDefaultAsync(b => b.Name.ToLower() == name.ToLower());
        }
        public async Task<IEnumerable<Badge>> GetBadgesByIdsAsync(IEnumerable<int> badgeIds)
        {
            return await _context.Badges
                .Where(b => badgeIds.Contains(b.Id))
                .ToListAsync();
        }


    }


}
