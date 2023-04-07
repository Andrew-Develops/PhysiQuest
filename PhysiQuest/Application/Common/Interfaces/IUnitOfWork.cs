using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IQuestRepository QuestRepository { get; }
        IBadgeRepository BadgeRepository { get; }
        IUserBadgeRepository UserBadgeRepository { get; }
        IUserQuestRepository UserQuestRepository { get; }
        Task<int> SaveChangesAsync();
    }

}
