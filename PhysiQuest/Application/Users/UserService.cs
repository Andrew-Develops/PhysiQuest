using Application.Badges.DTO;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Validator;
using Application.UserBadges.DTO;
using Application.UserQuests.DTO;
using Application.Users.DTO;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
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

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.UserRepository.GetUsersAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        /// <summary>
        /// Gets a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to get.</param>
        /// <returns>The user with the specified ID.</returns>
        /// <exception cref="UserNotFoundException">Thrown if no user with the specified ID is found.</exception>
        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException(id);
            }
            return _mapper.Map<UserDTO>(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userDto">The user DTO containing the user's information.</param>
        /// <returns>The newly created user.</returns>
        /// <exception cref="DuplicateEmailException">Thrown if a user with the same email already exists.</exception>
        public async Task<UserDTO> CreateUserAsync(CreateAndUpdateDTO userDto)
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

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="userDto">The user DTO containing the updated user's information.</param>
        /// <returns>The updated user.</returns>
        /// <exception cref="UserNotFoundException">Thrown if no user with the specified ID is found.</exception>
        /// <exception cref="DuplicateEmailException">Thrown if a user with the same email already exists.</exception>
        public async Task<UserDTO> UpdateUserAsync(int id, CreateAndUpdateDTO userDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new UserNotFoundException(id);
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

        /// <summary>
        /// Deletes a user with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>True if the user was deleted, false otherwise.</returns>
        /// <exception cref="UserNotFoundException">Thrown if no user with the specified ID is found.</exception>
        /// <exception cref="UserDeletionException">Thrown if the user deletion failed.</exception>
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
                throw new UserDeletionException($"Failed to delete user with id {id}.");
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets a list of all users sorted by their points in descending order.
        /// </summary>
        /// <returns>A list of users sorted by points in descending order.</returns>
        public async Task<IEnumerable<UserDTO>> GetUsersByPointsDescendingAsync()
        {
            var users = await _unitOfWork.UserRepository.GetUsersByPointsDescendingAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserBadgeDTO> AddBadgeToUserAsync(AssignBadgeDTO assignBadgeDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(assignBadgeDto.UserId);
            if (user == null)
            {
                throw new UserNotFoundException($"User with ID {assignBadgeDto.UserId} not found.");
            }

            var badge = await _unitOfWork.BadgeRepository.GetBadgeByIdAsync(assignBadgeDto.BadgeId);
            if (badge == null)
            {
                throw new BadgeNotFoundException($"Badge with ID {assignBadgeDto.BadgeId} not found.");
            }

            var userBadge = await _unitOfWork.UserBadgeRepository.GetUserBadgeByIdAsync(assignBadgeDto.UserId, assignBadgeDto.BadgeId);
            if (userBadge != null)
            {
                throw new InvalidOperationException($"User with ID {assignBadgeDto.UserId} already has the badge with ID {assignBadgeDto.BadgeId}.");
            }

            userBadge = new UserBadge
            {
                UserId = assignBadgeDto.UserId,
                BadgeId = assignBadgeDto.BadgeId,
                AwardDate = DateTime.UtcNow
            };

            await _unitOfWork.UserBadgeRepository.CreateUserBadgeAsync(userBadge);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserBadgeDTO>(userBadge);
        }


        public async Task<IEnumerable<BadgeDTO>> GetUserBadgesByNameAsync(string userName)
        {
            var user = await _unitOfWork.UserRepository.GetUserByNameAsync(userName);
            if (user == null)
            {
                throw new UserNameNotFoundException(userName);
            }

            var userBadges = await _unitOfWork.UserBadgeRepository.GetUserBadgesByUserIdAsync(user.Id);
            var badgeIds = userBadges.Select(ub => ub.BadgeId);
            var badges = await _unitOfWork.BadgeRepository.GetBadgesByIdsAsync(badgeIds);

            return _mapper.Map<IEnumerable<BadgeDTO>>(badges);
        }

        // UserService.cs
        public async Task<UserQuestDTO> AssignQuestToUserAsync(string username, int questId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByNameAsync(username);
            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            var quest = await _unitOfWork.QuestRepository.GetQuestByIdAsync(questId);
            if (quest == null)
            {
                throw new QuestNotFoundException(questId);
            }

            var userQuest = new UserQuest
            {
                UserId = user.Id,
                User = user,
                QuestId = quest.Id,
                Quest = quest,
                Status = "Assigned"
            };

            await _unitOfWork.UserQuestRepository.CreateUserQuestAsync(userQuest);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserQuestDTO>(userQuest);
        }

    }
}
