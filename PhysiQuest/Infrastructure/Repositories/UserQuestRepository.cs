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
    public class UserQuestRepository : IUserQuestRepository
    {
        private readonly ApplicationDbContext _context;

        public UserQuestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserQuest>> GetUserQuestsAsync()
        {
            return await _context.UserQuest.ToListAsync();
        }

        public async Task<UserQuest> GetUserQuestByIdAsync(int id)
        {
            return await _context.UserQuest.FindAsync(id);
        }

        public async Task<UserQuest> CreateUserQuestAsync(UserQuest userQuest)
        {
            _context.UserQuest.Add(userQuest);
            await _context.SaveChangesAsync();
            return userQuest;
        }

        public async Task<UserQuest> UpdateUserQuestAsync(UserQuest userQuest)
        {
            _context.Entry(userQuest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return userQuest;
        }

        public async Task<bool> DeleteUserQuestAsync(int id)
        {
            var userQuest = await _context.UserQuest.FindAsync(id);
            if (userQuest == null)
            {
                return false;
            }

            _context.UserQuest.Remove(userQuest);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserQuest> CompleteUserQuestAsync(string username, int questId)
        {
            var user = await _context.Users.Include(u => u.UserBadges).ThenInclude(ub => ub.Badge).FirstOrDefaultAsync(u => u.Name == username);

            if (user == null)
            {
                return null;
            }

            var userQuest = await _context.UserQuest.Include(uq => uq.Quest).FirstOrDefaultAsync(uq => uq.UserId == user.Id && uq.QuestId == questId);

            if (userQuest != null && userQuest.Status != "Completed")
            {
                userQuest.Status = "Completed";
                user.Points += userQuest.Quest.RewardPoints;
                user.Tokens += userQuest.Quest.RewardTokens;

                // Count the number of completed quests
                int completedQuests = _context.UserQuest.Count(uq => uq.UserId == user.Id && uq.Status == "Completed");

                // Increment the completedQuests count by 1 to account for the current quest
                completedQuests++;

                // Assign badges based on the number of completed quests
                if (completedQuests == 1)
                {
                    await AssignBadgeAsync(user, 1); // Beginner badge
                }
                else if (completedQuests == 5)
                {
                    await AssignBadgeAsync(user, 2); // Intermediate badge
                }
                else if (completedQuests == 10)
                {
                    await AssignBadgeAsync(user, 3); // Advanced badge
                }

                await _context.SaveChangesAsync();
            }

            return userQuest;
        }


        private async Task AssignBadgeAsync(User user, int badgeId)
        {
            if (!user.UserBadges.Any(ub => ub.BadgeId == badgeId))
            {
                var badge = await _context.Badges.FindAsync(badgeId);
                if (badge != null)
                {
                    var userBadge = new UserBadge
                    {
                        User = user,
                        Badge = badge
                    };

                    user.UserBadges.Add(userBadge);
                }
            }
        }



        public async Task<IEnumerable<UserQuest>> GetUserQuestsAsync(string username)
        {
            return await _context.UserQuest
                .Include(uq => uq.User)
                .Include(uq => uq.Quest)
                .Where(uq => uq.User.Name == username)
                .ToListAsync();
        }

        public async Task<UserQuest> DeleteUserQuestAsync(string username, int questId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == username);

            if (user == null)
            {
                return null;
            }

            var userQuest = await _context.UserQuest.Include(uq => uq.Quest).FirstOrDefaultAsync(uq => uq.UserId == user.Id && uq.QuestId == questId);

            if (userQuest != null)
            {
                if (userQuest.Status == "Completed")
                {
                    user.Points -= userQuest.Quest.RewardPoints;
                    user.Tokens -= userQuest.Quest.RewardTokens;
                }

                _context.UserQuest.Remove(userQuest);
                await _context.SaveChangesAsync();
            }

            return userQuest;
        }


    }


}
