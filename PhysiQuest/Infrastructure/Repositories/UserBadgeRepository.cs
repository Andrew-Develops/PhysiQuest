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
    public class UserBadgeRepository : IUserBadgeRepository
    {
        private readonly ApplicationDbContext _context;

        public UserBadgeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserBadge>> GetUserBadgesAsync()
        {
            return await _context.UserBadges.ToListAsync();
        }

        public async Task<UserBadge> GetUserBadgeByIdAsync(int userId, int badgeId)
        {
            return await _context.UserBadges.FindAsync(userId, badgeId);
        }

        public async Task<UserBadge> CreateUserBadgeAsync(UserBadge userBadge)
        {
            _context.UserBadges.Add(userBadge);
            await _context.SaveChangesAsync();
            return userBadge;
        }

        public async Task<UserBadge> UpdateUserBadgeAsync(UserBadge userBadge)
        {
            _context.Entry(userBadge).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return userBadge;
        }

        public async Task<bool> DeleteUserBadgeAsync(int userId, int badgeId)
        {
            var userBadge = await _context.UserBadges.FindAsync(userId, badgeId);
            if (userBadge == null)
            {
                return false;
            }

            _context.UserBadges.Remove(userBadge);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserBadge>> GetUserBadgesByUserIdAsync(int userId)
        {
            return await _context.UserBadges
                .Where(ub => ub.UserId == userId)
                .ToListAsync();
        }

        public async Task<UserBadge> DeleteUserBadgeAsync(string username, int badgeId)
        {
            var user = await _context.Users.Include(u => u.UserBadges).FirstOrDefaultAsync(u => u.Name == username);

            if (user == null)
            {
                return null;
            }

            var userBadge = user.UserBadges.FirstOrDefault(ub => ub.BadgeId == badgeId);

            if (userBadge != null)
            {
                _context.UserBadges.Remove(userBadge);
                await _context.SaveChangesAsync();
            }

            return userBadge;
        }

    }

}
