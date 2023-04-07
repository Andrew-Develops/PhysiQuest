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
    }


}
