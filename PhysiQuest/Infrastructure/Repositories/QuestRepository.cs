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
    public class QuestRepository : IQuestRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Quest>> GetQuestsAsync()
        {
            return await _context.Quests.ToListAsync();
        }

        public async Task<Quest> GetQuestByIdAsync(int id)
        {
            return await _context.Quests.FindAsync(id);
        }

        public async Task<Quest> CreateQuestAsync(Quest quest)
        {
            _context.Quests.Add(quest);
            await _context.SaveChangesAsync();
            return quest;
        }

        public async Task<Quest> UpdateQuestAsync(Quest quest)
        {
            _context.Entry(quest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return quest;
        }

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
    }

}
