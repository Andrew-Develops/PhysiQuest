using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserBadgeRepository : IUserBadgeRepository
    {
        private readonly ApplicationDbContext _context;

        public UserBadgeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all user badges from the database.
        /// </summary>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is an IEnumerable of UserBadge representing all user badges in the database.</returns>
        public async Task<IEnumerable<UserBadge>> GetUserBadgesAsync()
        {
            return await _context.UserBadges.ToListAsync();
        }

        /// <summary>
        /// Retrieves a user badge from the database by user ID and badge ID.
        /// </summary>
        /// <param name="userId">The ID of the user associated with the user badge.</param>
        /// <param name="badgeId">The ID of the badge associated with the user badge.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is the UserBadge object corresponding to the specified user ID and badge ID, or null if the specified user badge is not found in the database.</returns>
        public async Task<UserBadge> GetUserBadgeByIdAsync(int userId, int badgeId)
        {
            return await _context.UserBadges.FindAsync(userId, badgeId);
        }

        /// <summary>
        /// Adds a new user badge to the database.
        /// </summary>
        /// <param name="userBadge">The UserBadge object to be added.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is the UserBadge object that was added to the database.</returns>
        public async Task<UserBadge> CreateUserBadgeAsync(UserBadge userBadge)
        {
            _context.UserBadges.Add(userBadge);
            await _context.SaveChangesAsync();
            return userBadge;
        }

        /// <summary>
        /// Updates an existing user badge in the database.
        /// </summary>
        /// <param name="userBadge">The UserBadge object to be updated.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is the updated UserBadge object.</returns>
        public async Task<UserBadge> UpdateUserBadgeAsync(UserBadge userBadge)
        {
            _context.Entry(userBadge).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return userBadge;
        }

        /// <summary>
        /// Deletes a user badge from the database by user ID and badge ID.
        /// </summary>
        /// <param name="userId">The ID of the user associated with the user badge.</param>
        /// <param name="badgeId">The ID of the badge associated with the user badge.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is true if the user badge was successfully deleted, or false if the specified user badge is not found in the database.</returns>
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

        /// <summary>
        /// Retrieves all user badges from the database by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user associated with the user badges.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is an IEnumerable of UserBadge representing all user badges associated with the specified user ID.</returns>
        public async Task<IEnumerable<UserBadge>> GetUserBadgesByUserIdAsync(int userId)
        {
            return await _context.UserBadges
                .Where(ub => ub.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Deletes a user badge from the database by username and badge ID.
        /// </summary>
        /// <param name="username">The name of the user associated with the user badge.</param>
        /// <param name="badgeId">The ID of the badge associated with the user badge.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is the UserBadge object that was deleted from the database, or null if the specified user or badge ID is not found in the database.</returns>
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
