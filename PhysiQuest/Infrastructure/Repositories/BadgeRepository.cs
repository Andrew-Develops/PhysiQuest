using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BadgeRepository : IBadgeRepository
    {
        private readonly ApplicationDbContext _context;

        public BadgeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all badges from the database.
        /// </summary>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is an IEnumerable of Badge representing all badges in the database.</returns>
        public async Task<IEnumerable<Badge>> GetBadgesAsync()
        {
            return await _context.Badges.ToListAsync();
        }

        /// <summary>
        /// Retrieves a badge from the database by ID.
        /// </summary>
        /// <param name="id">The ID of the badge being retrieved.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is the Badge object corresponding to the specified ID, or null if the ID is not found in the database.</returns>
        public async Task<Badge> GetBadgeByIdAsync(int id)
        {
            return await _context.Badges.FindAsync(id);
        }

        /// <summary>
        /// Adds a new badge to the database.
        /// </summary>
        /// <param name="badge">The Badge object to be added.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is the Badge object that was added to the database.</returns>
        public async Task<Badge> CreateBadgeAsync(Badge badge)
        {
            _context.Badges.Add(badge);
            await _context.SaveChangesAsync();
            return badge;
        }

        /// <summary>
        /// Updates an existing badge in the database.
        /// </summary>
        /// <param name="badge">The Badge object to be updated.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is the updated Badge object.</returns>
        public async Task<Badge> UpdateBadgeAsync(Badge badge)
        {
            _context.Entry(badge).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return badge;
        }

        /// <summary>
        /// Deletes a badge from the database by ID.
        /// </summary>
        /// <param name="id">The ID of the badge being deleted.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is true if the badge was successfully deleted, or false if the specified ID was not found in the database.</returns>
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

        /// <summary>
        /// Retrieves a badge from the database by name.
        /// </summary>
        /// <param name="name">The name of the badge being retrieved.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is the Badge object corresponding to the specified name, or null if the name is not found in the database.</returns>
        public async Task<Badge> GetBadgeByNameAsync(string name)
        {
            return await _context.Badges.FirstOrDefaultAsync(b => b.Name.ToLower() == name.ToLower());
        }

        /// <summary>
        /// Retrieves a list of badges from the database by their IDs.
        /// </summary>
        /// <param name="badgeIds">The list of IDs of the badges being retrieved.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is an IEnumerable of Badge representing the badges corresponding to the specified IDs.</returns>
        public async Task<IEnumerable<Badge>> GetBadgesByIdsAsync(IEnumerable<int> badgeIds)
        {
            return await _context.Badges
                .Where(b => badgeIds.Contains(b.Id))
                .ToListAsync();
        }
    }
}
