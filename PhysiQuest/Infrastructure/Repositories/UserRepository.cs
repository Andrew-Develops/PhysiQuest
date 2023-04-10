using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a collection of all users in the database.
        /// </summary>
        /// <returns>A collection of all users in the database.</returns>
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Returns a user from the database by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user with the specified ID, or null if the user does not exist.</returns>
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        /// <summary>
        /// Creates a new user in the database.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>The newly created user.</returns>
        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Updates a user in the database.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <returns>The updated user.</returns>
        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Deletes a user from the database by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>True if the user was deleted, false if the user does not exist.</returns>
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Returns a user from the database by their email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>The user with the specified email, or null if the user does not exist.</returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Returns a collection of all users in the database sorted by their points in descending order.
        /// </summary>
        /// <returns>A collection of all users in the database sorted by their points in descending order.</returns>
        public async Task<IEnumerable<User>> GetUsersByPointsDescendingAsync()
        {
            return await _context.Users.OrderByDescending(u => u.Points).ToListAsync();
        }

        /// <summary>
        /// Returns a user from the database by their username, including their badges and quests.
        /// </summary>
        /// <param name="userName">The username of the user to retrieve.</param>
        /// <returns>The user with the specified username, or null if the user does not exist.</returns>
        public async Task<User> GetUserByNameAsync(string userName)
        {
            return await _context.Users
                .Include(u => u.UserBadges)
                .Include(u => u.UserQuests)
                .FirstOrDefaultAsync(u => u.Name == userName);
        }

        /// <summary>
        /// Retrieves a user from the database using the specified username.
        /// </summary>
        /// <param name="userName">The username of the user to retrieve.</param>
        /// <returns>A User object representing the retrieved user, or null if the user is not found.</returns>
        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Name == userName);
        }

    }

}
