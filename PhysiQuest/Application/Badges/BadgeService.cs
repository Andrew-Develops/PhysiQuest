using Application.Badges.DTO;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Badges
{
    public class BadgeService : IBadgeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BadgeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all badges.
        /// </summary>
        /// <returns>An IEnumerable of BadgeDTO representing all the badges.</returns>
        public async Task<IEnumerable<BadgeDTO>> GetAllBadgesAsync()
        {
            var badges = await _unitOfWork.BadgeRepository.GetBadgesAsync();
            return _mapper.Map<IEnumerable<BadgeDTO>>(badges);
        }

        /// <summary>
        /// Retrieves a badge by its id.
        /// </summary>
        /// <param name="id">The id of the badge to retrieve.</param>
        /// <returns>A BadgeDTO representing the badge with the specified id.</returns>
        /// <exception cref="BadgeNotFoundException">Thrown when no badge with the specified id is found.</exception>
        public async Task<BadgeDTO> GetBadgeByIdAsync(int id)
        {
            var badge = await _unitOfWork.BadgeRepository.GetBadgeByIdAsync(id);
            if (badge == null)
            {
                throw new BadgeNotFoundException(id);
            }
            return _mapper.Map<BadgeDTO>(badge);
        }

        /// <summary>
        /// Creates a new badge.
        /// </summary>
        /// <param name="badgeDto">A CreateAndUpdateBadgeDTO representing the badge to be created.</param>
        /// <returns>A BadgeDTO representing the newly created badge.</returns>
        /// <exception cref="DuplicateBadgeException">Thrown when a badge with the same name already exists.</exception>
        public async Task<BadgeDTO> CreateBadgeAsync(CreateAndUpdateBadgeDTO badgeDto)
        {
            var existingBadge = await _unitOfWork.BadgeRepository.GetBadgeByNameAsync(badgeDto.Name);
            if (existingBadge != null)
            {
                throw new DuplicateBadgeException($"Badge with name {badgeDto.Name} already exists.");
            }

            var badge = _mapper.Map<Badge>(badgeDto);
            await _unitOfWork.BadgeRepository.CreateBadgeAsync(badge);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<BadgeDTO>(badge);
        }

        /// <summary>
        /// Updates an existing badge.
        /// </summary>
        /// <param name="id">The id of the badge to update.</param>
        /// <param name="badgeDto">A CreateAndUpdateBadgeDTO representing the new values for the badge.</param>
        /// <returns>A BadgeDTO representing the updated badge.</returns>
        /// <exception cref="BadgeNotFoundException">Thrown when no badge with the specified id is found.</exception>
        /// <exception cref="DuplicateBadgeException">Thrown when a badge with the same name already exists (excluding the badge being updated).</exception>
        public async Task<BadgeDTO> UpdateBadgeAsync(int id, CreateAndUpdateBadgeDTO badgeDto)
        {
            var badge = await _unitOfWork.BadgeRepository.GetBadgeByIdAsync(id);

            if (badge == null)
            {
                throw new BadgeNotFoundException(id);
            }

            var existingBadge = await _unitOfWork.BadgeRepository.GetBadgeByNameAsync(badgeDto.Name);
            if (existingBadge != null && existingBadge.Id != id)
            {
                throw new DuplicateBadgeException($"Badge with name {badgeDto.Name} already exists.");
            }

            _mapper.Map(badgeDto, badge);
            await _unitOfWork.BadgeRepository.UpdateBadgeAsync(badge);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BadgeDTO>(badge);
        }

        /// <summary>
        /// Deletes a badge by its id.
        /// </summary>
        /// <param name="id">The id of the badge to delete.</param>
        /// <returns>A boolean value indicating whether the deletion was successful.</returns>
        /// <exception cref="BadgeNotFoundException">Thrown when no badge with the specified id is found.</exception>
        /// <exception cref="BadgeDeletionFailedException">Thrown when the deletion of the badge fails.</exception>
        public async Task<bool> DeleteBadgeAsync(int id)
        {
            var badge = await _unitOfWork.BadgeRepository.GetBadgeByIdAsync(id);
            if (badge == null)
            {
                throw new BadgeNotFoundException(id);
            }

            var result = await _unitOfWork.BadgeRepository.DeleteBadgeAsync(id);
            if (!result)
            {
                throw new BadgeDeletionFailedException(id);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }

}
