using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{

    public class QuestRepository : IQuestRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all quests from the database.
        /// </summary>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is an IEnumerable of Quest representing all quests in the database.</returns>
        public async Task<IEnumerable<Quest>> GetQuestsAsync()
        {
            return await _context.Quests.ToListAsync();
        }

        /// <summary>
        /// Retrieves a quest from the database by ID.
        /// </summary>
        /// <param name="id">The ID of the quest being retrieved.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is the Quest object corresponding to the specified ID, or null if the ID is not found in the database.</returns>
        public async Task<Quest> GetQuestByIdAsync(int id)
        {
            return await _context.Quests.FindAsync(id);
        }

        /// <summary>
        /// Adds a new quest to the database.
        /// </summary>
        /// <param name="quest">The Quest object to be added.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is the Quest object that was added to the database.</returns>
        public async Task<Quest> CreateQuestAsync(Quest quest)
        {
            _context.Quests.Add(quest);
            await _context.SaveChangesAsync();
            return quest;
        }

        /// <summary>
        /// Updates an existing quest in the database.
        /// </summary>
        /// <param name="quest">The Quest object to be updated.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is the updated Quest object.</returns>
        public async Task<Quest> UpdateQuestAsync(Quest quest)
        {
            _context.Entry(quest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return quest;
        }

        /// <summary>
        /// Deletes a quest from the database by ID.
        /// </summary>
        /// <param name="id">The ID of the quest being deleted.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is true if the quest was successfully deleted, or false if the specified ID was not found in the database.</returns>
        public async Task<bool> DeleteQuestAsync(int id)
        {
            var quest = await _context.Quests.FindAsync(id);
            if (quest == null)
            {
                return false;
            }

            _context.Quests.Remove(quest);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves a quest from the database by title.
        /// </summary>
        /// <param name="title">The title of the quest being retrieved.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is the Quest object corresponding to the specified title, or null if the title is not found in the database.</returns>
        public async Task<Quest> GetQuestByTitleAsync(string title)
        {
            return await _context.Quests
                .Where(q => EF.Functions.Like(q.Title, title))
                .FirstOrDefaultAsync();
        }


        //public async Task<UserQuest> CompleteUserQuestAsync(string username, int questId)
        //{
        //    var userQuest = await _context.UserQuest
        //        .Include(uq => uq.User)
        //        .Include(uq => uq.Quest)
        //        .FirstOrDefaultAsync(uq => uq.User.Name == username && uq.QuestId == questId);

        //    if (userQuest == null)
        //    {
        //        return null;
        //    }

        //    userQuest.Status = "Completed";
        //    await _context.SaveChangesAsync();

        //    return userQuest;
        //}


        /// <summary>
        /// Retrieves all quests from the database in alphabetical order.
        /// </summary>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is a List of Quest representing all quests in the database, sorted alphabetically by title.</returns>
        public async Task<List<Quest>> GetQuestsAlphabeticalAsync()
        {
            return await _context.Quests.OrderBy(q => q.Title).ToListAsync();
        }

        /// <summary>
        /// Retrieves all quests from the database by reward points, in descending order.
        /// </summary>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is a List of Quest representing all quests in the database, sorted by reward points in descending order.</returns>
        public async Task<List<Quest>> GetQuestsByRewardPointsAsync()
        {
            return await _context.Quests.OrderByDescending(q => q.RewardPoints).ToListAsync();
        }

        /// <summary>
        /// Retrieves all quests from the database by reward tokens, in descending order.
        /// </summary>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is a List of Quest representing all quests in the database, sorted by reward tokens in descending order.</returns>
        public async Task<List<Quest>> GetQuestsByRewardTokensAsync()
        {
            return await _context.Quests.OrderByDescending(q => q.RewardTokens).ToListAsync();
        }
    }
}
