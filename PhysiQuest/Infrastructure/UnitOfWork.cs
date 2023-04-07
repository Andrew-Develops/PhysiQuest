using Application.Common.Interfaces;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IUserRepository _userRepository;
        private IQuestRepository _questRepository;
        private IBadgeRepository _badgeRepository;
        private IUserBadgeRepository _userBadgeRepository;
        private IUserQuestRepository _userQuestRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository ??= new UserRepository(_context);
            }
        }

        public IQuestRepository QuestRepository
        {
            get
            {
                return _questRepository ??= new QuestRepository(_context);
            }
        }

        public IBadgeRepository BadgeRepository
        {
            get
            {
                return _badgeRepository ??= new BadgeRepository(_context);
            }
        }
        public IUserBadgeRepository UserBadgeRepository
        {
            get
            {
                return _userBadgeRepository ??= new UserBadgeRepository(_context);
            }
        }

        public IUserQuestRepository UserQuestRepository
        {
            get
            {
                return _userQuestRepository ??= new UserQuestRepository(_context);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
