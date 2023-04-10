using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserQuestRepository : IUserQuestRepository
    {
        private readonly ApplicationDbContext _context;

        public UserQuestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all user quests from the database.
        /// </summary>
        /// <returns>An asynchronous task that returns a list of UserQuest objects.</returns>
        public async Task<IEnumerable<UserQuest>> GetUserQuestsAsync()
        {
            return await _context.UserQuest.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific user quest from the database by ID.
        /// </summary>
        /// <param name="id">The ID of the user quest to retrieve.</param>
        /// <returns>An asynchronous task that returns a UserQuest object, or null if the quest is not found.</returns>
        public async Task<UserQuest> GetUserQuestByIdAsync(int id)
        {
            return await _context.UserQuest.FindAsync(id);
        }

        /// <summary>
        /// Adds a new user quest to the database.
        /// </summary>
        /// <param name="userQuest">The UserQuest object to add to the database.</param>
        /// <returns>An asynchronous task that returns the added UserQuest object.</returns>
        public async Task<UserQuest> CreateUserQuestAsync(UserQuest userQuest)
        {
            _context.UserQuest.Add(userQuest);
            await _context.SaveChangesAsync();
            return userQuest;
        }

        /// <summary>
        /// Updates an existing user quest in the database.
        /// </summary>
        /// <param name="userQuest">The UserQuest object to update in the database.</param>
        /// <returns>An asynchronous task that returns the updated UserQuest object.</returns>
        public async Task<UserQuest> UpdateUserQuestAsync(UserQuest userQuest)
        {
            _context.Entry(userQuest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return userQuest;
        }

        /// <summary>
        /// Removes a user quest from the database by ID.
        /// </summary>
        /// <param name="id">The ID of the user quest to remove from the database.</param>
        /// <returns>An asynchronous task that returns true if the quest was successfully deleted, false otherwise.</returns>
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

        /// <summary>
        /// Completes a user quest and assigns rewards to the user.
        /// </summary>
        /// <param name="username">The username of the user completing the quest.</param>
        /// <param name="questId">The ID of the quest being completed.</param>
        /// <param name="imageUrl">The URL of the image that serves as proof of completion.</param>
        /// <returns>A UserQuest object representing the completed quest, or null if the user is not found.</returns>
        public async Task<UserQuest> CompleteUserQuestAsync(string username, int questId, string imageUrl)
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
                userQuest.ProofImage = imageUrl;

                // Count the number of completed quests
                int completedQuests = _context.UserQuest.Count(uq => uq.UserId == user.Id && uq.Status == "Completed");

                // Increment the completedQuests count by 1 to account for the current quest
                completedQuests++;

                // Assign badges based on the number of completed quests
                if (completedQuests == 1)
                {
                    await AssignBadgeAsync(user, 1);
                }
                else if (completedQuests == 5)
                {
                    await AssignBadgeAsync(user, 2);
                }
                else if (completedQuests == 10)
                {
                    await AssignBadgeAsync(user, 3);
                }

                // Assign badges based on the user's points
                if (user.Points >= 15000)
                {
                    await AssignBadgeAsync(user, 8);
                }
                else if (user.Points >= 5000)
                {
                    await AssignBadgeAsync(user, 7);
                }
                else if (user.Points >= 1000)
                {
                    await AssignBadgeAsync(user, 6);
                }
                else if (user.Points >= 500)
                {
                    await AssignBadgeAsync(user, 5);
                }

                await _context.SaveChangesAsync();
            }

            return userQuest;
        }

        /// <summary>
        /// Assigns a badge to a user if they haven't already earned it.
        /// </summary>
        /// <param name="user">The User object to assign the badge to.</param>
        /// <param name="badgeId">The ID of the badge to assign to the user.</param>
        /// <returns>An asynchronous task.</returns>
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

        /// <summary>
        /// Retrieves a list of all user quests for a specific user from the database.
        /// </summary>
        /// <param name="username">The name of the user to retrieve quests for.</param>
        /// <returns>An asynchronous task that returns a list of UserQuest objects for the specified user.</returns>
        public async Task<IEnumerable<UserQuest>> GetUserQuestsAsync(string username)
        {
            return await _context.UserQuest
                .Include(uq => uq.User)
                .Include(uq => uq.Quest)
                .Where(uq => uq.User.Name == username)
                .ToListAsync();
        }

        /// <summary>
        /// Removes a user quest from the database by the user's name and the quest's ID.
        /// If the quest was completed, also updates the associated user's points and tokens.
        /// </summary>
        /// <param name="username">The name of the user who completed the quest.</param>
        /// <param name="questId">The ID of the quest to remove from the database.</param>
        /// <returns>An asynchronous task that returns the removed UserQuest object, or null if the user or quest is not found.</returns>
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

        /// <summary>
        /// Retrieves a user quest from the database using the specified user ID and quest ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="questId">The ID of the quest.</param>
        /// <returns>A UserQuest object representing the user quest, or null if no such user quest is found.</returns>
        public async Task<UserQuest> GetUserQuestByUserIdAndQuestIdAsync(int userId, int questId)
        {
            return await _context.UserQuest.FirstOrDefaultAsync(uq => uq.UserId == userId && uq.QuestId == questId);
        }

        /// <summary>
        /// Retrieves the URL of the proof image associated with the specified user and quest.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="questId">The ID of the quest.</param>
        /// <returns>The URL of the proof image, or null if the user is not found or the quest has not been completed.</returns>
        public async Task<string> GetProofImageUrlAsync(string username, int questId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == username);

            if (user == null)
            {
                return null;
            }

            var userQuest = await _context.UserQuest.FirstOrDefaultAsync(uq => uq.UserId == user.Id && uq.QuestId == questId);

            if (userQuest != null && userQuest.Status == "Completed")
            {
                return userQuest.ProofImage;
            }

            return null;
        }

        /// <summary>
        /// Deletes the URL of the proof image associated with the specified user and quest.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="questId">The ID of the quest.</param>
        /// <returns>A UserQuest object representing the updated user quest, or null if the user is not found or the quest has not been completed.</returns>
        public async Task<UserQuest> DeleteProofImageUrlAsync(string username, int questId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == username);

            if (user == null)
            {
                return null;
            }

            var userQuest = await _context.UserQuest.FirstOrDefaultAsync(uq => uq.UserId == user.Id && uq.QuestId == questId);

            if (userQuest != null && userQuest.Status == "Completed")
            {
                userQuest.ProofImage = null;
                await _context.SaveChangesAsync();
            }

            return userQuest;
        }

    }
}
