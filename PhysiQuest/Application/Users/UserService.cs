using Application.Badges.DTO;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Quests.DTO;
using Application.UserBadges.DTO;
using Application.UserQuests.DTO;
using Application.Users.DTO;
using AutoMapper;
using Domain.Entities;
using System.Data;

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
        /// Retrieves a user from the database using the specified username.
        /// </summary>
        /// <param name="userName">The username of the user to retrieve.</param>
        /// <returns>A UserDTO object representing the retrieved user.</returns>
        /// <exception cref="UserNotFoundException">Thrown if the user is not found.</exception>
        public async Task<UserDTO> GetUserByUserNameAsync(string userName)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(userName);
            if (user == null)
            {
                throw new UserNotFoundException($"User with username '{userName}' not found.");
            }
            return _mapper.Map<UserDTO>(user);
        }

        /// <summary>
        /// Creates a new user based on the information provided in the <paramref name="userDto"/> parameter.
        /// </summary>
        /// <param name="userDto">The data transfer object that contains the information needed to create the user.</param>
        /// <returns>The newly created user as a <see cref="UserDTO"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when either the email or username is null or empty.</exception>
        /// <exception cref="DuplicateEmailException">Thrown when a user with the same email address already exists in the system.</exception>
        /// <exception cref="DuplicateNameException">Thrown when a user with the same username already exists in the system.</exception>
        public async Task<UserDTO> CreateUserAsync(CreateAndUpdateDTO userDto)
        {

            if (string.IsNullOrEmpty(userDto.Email))
            {
                throw new ArgumentException("Email is required.", nameof(userDto.Email));
            }

            if (string.IsNullOrEmpty(userDto.Name))
            {
                throw new ArgumentException("Username is required.", nameof(userDto.Name));
            }

            var userWithEmail = await _unitOfWork.UserRepository.GetUserByEmailAsync(userDto.Email);
            if (userWithEmail != null)
            {
                throw new DuplicateEmailException($"User with email {userDto.Email} already exists.");
            }

            var userWithName = await _unitOfWork.UserRepository.GetUserByNameAsync(userDto.Name);
            if (userWithName != null)
            {
                throw new DuplicateNameException($"User with name {userDto.Name} already exists.");
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
                throw new UserNotFoundException($"User with ID {id} not found.");
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

        /// <summary>
        /// Assigns a badge to a user.
        /// </summary>
        /// <param name="assignBadgeDto">The DTO containing the user and badge IDs.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is the DTO representation of the newly assigned UserBadge.</returns>
        /// <exception cref="UserNotFoundException">Thrown when the specified user ID is not found in the database.</exception>
        /// <exception cref="BadgeNotFoundException">Thrown when the specified badge ID is not found in the database.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the specified user already has the specified badge assigned to them.</exception>
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

        /// <summary>
        /// Gets all badges assigned to a user by their username.
        /// </summary>
        /// <param name="userName">The username of the user.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is an IEnumerable of BadgeDTO representing the badges assigned to the user.</returns>
        /// <exception cref="UserNameNotFoundException">Thrown when the specified username is not found in the database.</exception>
        public async Task<IEnumerable<BadgeDTO>> GetUserBadgesByNameAsync(string userName)
        {
            var user = await _unitOfWork.UserRepository.GetUserByNameAsync(userName);
            if (user == null)
            {
                throw new UserNameNotFoundException($"The user with username '{userName}' was not found.");
            }

            var userBadges = await _unitOfWork.UserBadgeRepository.GetUserBadgesByUserIdAsync(user.Id);
            var badgeIds = userBadges.Select(ub => ub.BadgeId);
            var badges = await _unitOfWork.BadgeRepository.GetBadgesByIdsAsync(badgeIds);

            return _mapper.Map<IEnumerable<BadgeDTO>>(badges);
        }

        /// <summary>
        /// Assigns a quest to a user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="questId">The ID of the quest being assigned.</param>
        /// <returns>Returns a Task representing the asynchronous operation. The result of the task is the DTO representation of the newly assigned UserQuest.</returns>
        /// <exception cref="UserNotFoundException">Thrown when the specified username is not found in the database.</exception>
        /// <exception cref="QuestNotFoundException">Thrown when the specified quest ID is not found in the database.</exception>
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
                throw new QuestNotFoundException($"The quest with ID {questId} could not be found.");
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

        /// <summary>
        /// Creates a new quest for the specified user.
        /// </summary>
        /// <param name="username">The username of the user for whom the quest is created.</param>
        /// <param name="questDto">The quest data.</param>
        /// <returns>The created quest.</returns>
        /// <exception cref="UserNotFoundException">Thrown when the user with the specified username is not found.</exception>
        /// <exception cref="InsufficientTokensException">Thrown when the user does not have enough tokens to create a quest.</exception>
        /// <exception cref="DuplicateQuestException">Thrown when a quest with the same title already exists.</exception>
        public async Task<QuestDTO> CreateUserQuestAsync(string username, CreateAndUpdateQuestDTO questDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByNameAsync(username);
            if (user == null)
            {
                throw new UserNotFoundException($"User with name {username} not found.");
            }

            if (user.Tokens < 20)
            {
                throw new InsufficientTokensException("User does not have enough tokens to create a quest.");
            }

            var existingQuest = await _unitOfWork.QuestRepository.GetQuestByTitleAsync(questDto.Title);
            if (existingQuest != null)
            {
                throw new DuplicateQuestException($"Quest with title {questDto.Title} already exists.");
            }

            // Set maximum allowed RewardPoints and RewardTokens
            questDto.RewardPoints = Math.Min(questDto.RewardPoints, 100);
            questDto.RewardTokens = Math.Min(questDto.RewardTokens, 25);

            var quest = _mapper.Map<Quest>(questDto);
            await _unitOfWork.QuestRepository.CreateQuestAsync(quest);

            // Deduct 20 tokens from the user's balance
            user.Tokens -= 20;

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<QuestDTO>(quest);
        }

        /// <summary>
        /// Retrieves the URL of the proof image associated with the specified user and quest.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="questId">The ID of the quest.</param>
        /// <returns>The URL of the proof image.</returns>
        /// <exception cref="UserQuestNotFoundException">Thrown if the proof image URL is not found.</exception>
        public async Task<string> GetProofImageUrlAsync(string username, int questId)
        {
            var proofImageUrl = await _unitOfWork.UserQuestRepository.GetProofImageUrlAsync(username, questId);

            if (proofImageUrl == null)
            {
                throw new UserQuestNotFoundException($"Proof image URL for user {username} and quest id {questId} not found.");
            }

            return proofImageUrl;
        }

        /// <summary>
        /// Deletes the URL of the proof image associated with the specified user and quest.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="questId">The ID of the quest.</param>
        /// <returns>A UserQuestDTO object representing the updated user quest.</returns>
        /// <exception cref="UserQuestNotFoundException">Thrown if the user quest is not found.</exception>
        public async Task<UserQuestDTO> DeleteProofImageUrlAsync(string username, int questId)
        {
            var userQuest = await _unitOfWork.UserQuestRepository.DeleteProofImageUrlAsync(username, questId);

            if (userQuest == null)
            {
                throw new UserQuestNotFoundException($"User quest with user name {username} and quest id {questId} not found.");
            }

            return _mapper.Map<UserQuestDTO>(userQuest);
        }


    }
}
