using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Quests.DTO;
using Application.UserQuests.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Quests
{
    public class QuestService : IQuestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all quests.
        /// </summary>
        /// <returns>An IEnumerable of QuestDTOs representing all quests.</returns>
        public async Task<IEnumerable<QuestDTO>> GetAllQuestsAsync()
        {
            var quests = await _unitOfWork.QuestRepository.GetQuestsAsync();
            return _mapper.Map<IEnumerable<QuestDTO>>(quests);
        }

        /// <summary>
        /// Retrieves a quest by ID.
        /// </summary>
        /// <param name="id">The ID of the quest to retrieve.</param>
        /// <returns>A QuestDTO representing the retrieved quest.</returns>
        /// <exception cref="QuestNotFoundException">Thrown if no quest with the specified ID is found.</exception>
        public async Task<QuestDTO> GetQuestByIdAsync(int id)
        {
            var quest = await _unitOfWork.QuestRepository.GetQuestByIdAsync(id);
            if (quest == null)
            {
                throw new QuestNotFoundException($"The quest with ID {id} could not be found.");
            }
            return _mapper.Map<QuestDTO>(quest);
        }

        /// <summary>
        /// Creates a new quest with the given details.
        /// </summary>
        /// <param name="questDto">The quest details to create.</param>
        /// <returns>The newly created quest.</returns>
        /// <exception cref="ArgumentException">Thrown when the quest title or description are empty or whitespace.</exception>
        /// <exception cref="DuplicateQuestException">Thrown when a quest with the same title already exists.</exception>
        public async Task<QuestDTO> CreateQuestAsync(CreateAndUpdateQuestDTO questDto)
        {
            if (string.IsNullOrWhiteSpace(questDto.Title))
            {
                throw new ArgumentException("Quest title cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(questDto.Description))
            {
                throw new ArgumentException("Quest description cannot be empty.");
            }

            var existingQuest = await _unitOfWork.QuestRepository.GetQuestByTitleAsync(questDto.Title);
            if (existingQuest != null)
            {
                throw new DuplicateQuestException($"Quest with title {questDto.Title} already exists.");
            }

            var quest = _mapper.Map<Quest>(questDto);
            await _unitOfWork.QuestRepository.CreateQuestAsync(quest);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<QuestDTO>(quest);
        }

        /// <summary>
        /// Updates an existing quest.
        /// </summary>
        /// <param name="id">The ID of the quest to update.</param>
        /// <param name="questDto">The updated quest data.</param>
        /// <returns>A QuestDTO representing the updated quest.</returns>
        /// <exception cref="QuestNotFoundException">Thrown if no quest with the specified ID is found.</exception>
        /// <exception cref="DuplicateQuestException">Thrown if a quest with the same title already exists (other than the one being updated).</exception>
        public async Task<QuestDTO> UpdateQuestAsync(int id, CreateAndUpdateQuestDTO questDto)
        {
            var quest = await _unitOfWork.QuestRepository.GetQuestByIdAsync(id);

            if (quest == null)
            {
                throw new QuestNotFoundException($"The quest with ID {id} could not be found.");
            }

            var existingQuest = await _unitOfWork.QuestRepository.GetQuestByTitleAsync(questDto.Title);
            if (existingQuest != null && existingQuest.Id != id)
            {
                throw new DuplicateQuestException($"Quest with title {questDto.Title} already exists.");
            }

            _mapper.Map(questDto, quest);
            await _unitOfWork.QuestRepository.UpdateQuestAsync(quest);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<QuestDTO>(quest);
        }

        /// <summary>
        /// Deletes a quest.
        /// </summary>
        /// <param name="id">The ID of the quest to delete.</param>
        /// <returns>True if the quest was deleted, false otherwise.</returns>
        /// <exception cref="QuestNotFoundException">Thrown if no quest with the specified ID is found.</exception>
        /// <exception cref="QuestDeletionFailedException">Thrown if the quest deletion failed.</exception>
        public async Task<bool> DeleteQuestAsync(int id)
        {
            var quest = await _unitOfWork.QuestRepository.GetQuestByIdAsync(id);
            if (quest == null)
            {
                throw new QuestNotFoundException($"The quest with ID {id} could not be found.");
            }

            var result = await _unitOfWork.QuestRepository.DeleteQuestAsync(id);
            if (!result)
            {
                throw new QuestDeletionFailedException($"Failed to delete quest with ID {id}");
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Completes a user quest for the specified user and quest ID.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="questId">The ID of the quest.</param>
        /// <returns>The completed user quest DTO, or null if the user quest was not found.</returns>
        public async Task<UserQuestDTO> CompleteUserQuestAsync(string username, int questId, string imageUrl)
        {
            var userQuest = await _unitOfWork.UserQuestRepository.CompleteUserQuestAsync(username, questId, imageUrl);

            if (userQuest == null)
            {
                throw new UserQuestNotFoundException($"User quest with user name {username} and quest id {questId} not found.");
            }

            return _mapper.Map<UserQuestDTO>(userQuest);
        }

        /// <summary>
        /// Gets the user quests for the specified user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The list of user quest detail DTOs.</returns>
        public async Task<IEnumerable<UserQuestDetailDTO>> GetUserQuestsAsync(string username)
        {
            var userQuests = await _unitOfWork.UserQuestRepository.GetUserQuestsAsync(username);
            return userQuests.Select(uq => new UserQuestDetailDTO
            {
                Title = uq.Quest.Title,
                Description = uq.Quest.Description,
                RewardPoints = uq.Quest.RewardPoints,
                RewardTokens = uq.Quest.RewardTokens,
                Status = uq.Status
            }).ToList();
        }

        /// <summary>
        /// Deletes a user quest for the specified user and quest ID.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="questId">The ID of the quest.</param>
        /// <returns>The deleted user quest DTO, or null if the user quest was not found.</returns>
        public async Task<UserQuestDTO> DeleteUserQuestAsync(string username, int questId)
        {
            var userQuest = await _unitOfWork.UserQuestRepository.DeleteUserQuestAsync(username, questId);
            return userQuest != null ? _mapper.Map<UserQuestDTO>(userQuest) : null;
        }

        /// <summary>
        /// Gets the quests in alphabetical order.
        /// </summary>
        /// <returns>The list of quest DTOs.</returns>
        public async Task<IEnumerable<QuestDTO>> GetQuestsAlphabeticalAsync()
        {
            var quest = await _unitOfWork.QuestRepository.GetQuestsAlphabeticalAsync();
            return _mapper.Map<List<QuestDTO>>(quest);

        }

        /// <summary>
        /// Gets the quests ordered by reward points.
        /// </summary>
        /// <returns>The list of quest DTOs.</returns>
        public async Task<IEnumerable<QuestDTO>> GetQuestsByRewardPointsAsync()
        {
            var quest = await _unitOfWork.QuestRepository.GetQuestsByRewardPointsAsync();
            return _mapper.Map<List<QuestDTO>>(quest);
        }

        /// <summary>
        /// Gets the quests ordered by reward tokens.
        /// </summary>
        /// <returns>The list of quest DTOs.</returns>
        public async Task<IEnumerable<QuestDTO>> GetQuestsByRewardTokensAsync()
        {
            var quest = await _unitOfWork.QuestRepository.GetQuestsByRewardTokensAsync();
            return _mapper.Map<List<QuestDTO>>(quest);
        }

    }
}
