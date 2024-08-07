﻿
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Repository.Repositories;
using Repository.Repositories.Interfaces;
using Service.DTOs.Blogs;
using Service.DTOs.Reviews;
using Service.Helpers;
using Service.Helpers.Exceptions;
using Service.Interfaces;

namespace Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPlaceRepository _placeRepository;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper, IPlaceRepository placeRepository,
                             UserManager<AppUser> userManager)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _placeRepository = placeRepository;
            _userManager = userManager;
        }

        public async Task CreateAsync(ReviewCreateDto model, string userId, int placeId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException("User ID is required.");
            }

            Review review = _mapper.Map<Review>(model);

            review.AppUser = await _userManager.FindByIdAsync(userId);
            review.Place = await _placeRepository.GetById(placeId);

            await _reviewRepository.CreateAsync(review);
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException();

            var review = await _reviewRepository.GetById((int)id);

            if (review is null) throw new NotFoundException("Review");

            await _reviewRepository.DeleteAsync(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetAllForPlace(int? placeId)
        {
            if (placeId is null) throw new ArgumentNullException();

            return _mapper.Map<IEnumerable<ReviewDto>>(await _reviewRepository.GetAllForPlace((int)placeId));
        }

        public async Task<IEnumerable<ReviewDto>> GetAllForUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException("User ID is required.");
            }

            return _mapper.Map<IEnumerable<ReviewDto>>(await _reviewRepository.GetAllForUser(userId));
        }

        public async Task<PaginateResponse<ReviewDto>> GetAllPaginated(int currentPage, int pageSize)
        {
            var response = await _reviewRepository.GetPagination(currentPage, pageSize);

            var result = new PaginateResponse<ReviewDto>()
            {
                Data = _mapper.Map<List<ReviewDto>>(response.Data),
            };

            return _mapper.Map(response, result);
        }

        public async Task<ReviewDto> GetByIdAsync(int id)
        {
            var review = await _reviewRepository.GetById((int)id);

            return review is null ? throw new NotFoundException("Review") : _mapper.Map<ReviewDto>(review);
        }
    }
}
