using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.BusinessLogic.Interfaces;
using UsedCarHub.Common.Errors;
using UsedCarHub.Common.Results;
using UsedCarHub.Domain.Entities;
using UsedCarHub.Repository.Interfaces;

namespace UsedCarHub.BusinessLogic.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IMapper _mapper;

        public AdvertisementService(IUnitOfWork unitOfWork, IAdvertisementRepository advertisementRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _advertisementRepository = advertisementRepository;
            _mapper = mapper;
        }
        
        public async Task<Result<AdvertisementDto>> AddAsync(AddAdvertisementDto addAdvertisementDto, string currentUserId)
        {
            CarEntity car = _mapper.Map<CarEntity>(addAdvertisementDto.Car);
            var advertisement = _mapper.Map<AdvertisementEntity>(addAdvertisementDto);
            advertisement.Car = car;
            advertisement.SellerId = currentUserId;
            var advertisementAddResult = await _advertisementRepository.AddAsync(advertisement);
            if (!advertisementAddResult.IsSuccess)
            {
                return Result<AdvertisementDto>.Failure(advertisementAddResult.ExecutionErrors);
            }

            var userGet =
                await _unitOfWork.UserManager.Users.FirstOrDefaultAsync(x => x.Id == advertisement.SellerId);
            if (userGet == null)
            {
                return Result<AdvertisementDto>.Failure(AccountError.NotFoundById);
            }

            userGet.Advertisements.Add(advertisement);
            if (!await _unitOfWork.Commit())
            {
                return Result<AdvertisementDto>.Failure(DbError.FailSaveChanges);
            }

            var advertisementDto = _mapper.Map<AdvertisementDto>(advertisement);
            return Result<AdvertisementDto>.Success(advertisementDto);
        }
        
        public async Task<Result<InfoAdvertisementDto>> GetInfoAsync(int advertisementId)
        {
            var getAdvertisementResult = await _advertisementRepository.GetAsync(advertisementId);
            if (getAdvertisementResult.IsSuccess)
            {
                var carAdvertisement = _mapper.Map<CarDto>(getAdvertisementResult.Value.Car);
                var advertisement = _mapper.Map<InfoAdvertisementDto>(getAdvertisementResult.Value);
                advertisement.Car = carAdvertisement;
                return Result<InfoAdvertisementDto>.Success(advertisement);
            }

            return Result<InfoAdvertisementDto>.Failure(getAdvertisementResult.ExecutionErrors);
        }
        
        public async Task<Result<List<InfoAdvertisementDto>>> GetAllInfoAsync()
        {
            var allAdvertisementsFromDb = await _advertisementRepository.GetAllAsync();
            var allAdvertisements = allAdvertisementsFromDb
                .Select(ad =>
                {
                    var advertisementDto = _mapper.Map<InfoAdvertisementDto>(ad);
                    advertisementDto.Car = _mapper.Map<CarDto>(ad.Car);
                    return advertisementDto;
                })
                .ToList();
            return Result<List<InfoAdvertisementDto>>.Success(allAdvertisements);
        }
        
        public async Task<Result<string>> UpdateAsync(int advertisementId, UpdateAdvertisementDto updateAdvertisementDto)
        {
            var advertisementEntity = _mapper.Map<AdvertisementEntity>(updateAdvertisementDto);
            var updateAdvertisementResult = await _advertisementRepository.UpdateAsync(advertisementId, advertisementEntity);
            if (updateAdvertisementResult.IsSuccess)
            {
                return Result<string>.Success(updateAdvertisementResult.Value);
            }

            return Result<string>.Failure(updateAdvertisementResult.ExecutionErrors);
        }

        public async Task<Result<string>> DeleteAsync(int advertisementId)
        {
            var resultDeleteAdvertisement = await _advertisementRepository.DeleteAsync(advertisementId);
            if (resultDeleteAdvertisement.IsSuccess)
            {
                return Result<string>.Success($"Advertisement with ID {advertisementId} was deleted");
            }

            return Result<string>.Failure(AdvertisementError.NotFoundById);
        }
    }
}