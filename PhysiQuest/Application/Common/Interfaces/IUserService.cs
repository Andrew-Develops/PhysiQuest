using Application.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<UserDTO> CreateUserAsync(UserDTO userDto);
        Task<UserDTO> UpdateUserAsync(int id, UserDTO userDto);
        Task<bool> DeleteUserAsync(int id);
    }
}
