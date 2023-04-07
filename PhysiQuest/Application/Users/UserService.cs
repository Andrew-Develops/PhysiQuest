using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Users.DTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.UserRepository.GetUsersAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException(id);
            }
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO userDto)
        {
            var userWithEmail = await _unitOfWork.UserRepository.GetUserByEmailAsync(userDto.Email);
            if (userWithEmail != null)
            {
                throw new DuplicateEmailException($"User with email {userDto.Email} already exists.");
            }

            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.UserRepository.CreateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> UpdateUserAsync(int id, UserDTO userDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new UserNotFoundException(id);
            }

            if (userDto.Name == null)
            {
                throw new ArgumentException("Name must not be null.");
            }

            if (userDto.Email == null)
            {
                throw new ArgumentException("Email must not be null.");
            }

            var userWithEmail = await _unitOfWork.UserRepository.GetUserByEmailAsync(userDto.Email);
            if (userWithEmail != null && userWithEmail.Id != id)
            {
                throw new DuplicateEmailException($"User with email {userDto.Email} already exists.");
            }

            _mapper.Map(userDto, user);
            await _unitOfWork.UserRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException(id);
            }

            var result = await _unitOfWork.UserRepository.DeleteUserAsync(id);
            if (!result)
            {
                throw new Exception($"Failed to delete user with id {id}.");
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersByPointsDescendingAsync()
        {
            var users = await _unitOfWork.UserRepository.GetUsersByPointsDescendingAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }
    }
}
