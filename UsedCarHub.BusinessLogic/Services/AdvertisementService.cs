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
        private IAdvertisementService _advertisementServiceImplementation;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IMapper _mapper;

        public AdvertisementService(IUnitOfWork unitOfWork, IAdvertisementRepository advertisementRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _advertisementRepository = advertisementRepository;
            _mapper = mapper;
        }
        
        public async Task<Result<AdvertisementDto>> AddAsync(AddAdvertisementDto addAdvertisementDto)
        {
            CarEntity car = _mapper.Map<CarEntity>(addAdvertisementDto.Car);
            var advertisement = _mapper.Map<AdvertisementEntity>(addAdvertisementDto);
            advertisement.Car = car;
            var advertisementAddResult = await _advertisementRepository.AddAsync(advertisement);
            if (!advertisementAddResult.IsSuccess)
                return Result<AdvertisementDto>.Failure(advertisementAddResult.ExecutionErrors);
            var userGet =
                await _unitOfWork.UserManager.Users.FirstOrDefaultAsync(x => x.Id == advertisement.SellerId);
            if (userGet == null)
                return Result<AdvertisementDto>.Failure(AccountError.NotFoundById);
            userGet.Advertisements.Add(advertisement);
            if (!await _unitOfWork.Commit())
                return Result<AdvertisementDto>.Failure(DbError.FailSaveChanges);
            var advertisementDto = _mapper.Map<AdvertisementDto>(advertisement);
            return Result<AdvertisementDto>.Success(advertisementDto);
        }

        public Task<Result<UpdateAdvertisementDto>> UpdateAsync(int advertisementId, UpdateAdvertisementDto updateAdvertisementDto)
        {
            return _advertisementServiceImplementation.UpdateAsync(advertisementId, updateAdvertisementDto);
        }

        public Task<Result<InfoAdvertisementDto>> GetInfoAsync(int advertisementId)
        {
            return _advertisementServiceImplementation.GetInfoAsync(advertisementId);
        }

        public Task<Result<string>> DeleteAsync(int advertisementId)
        {
            return _advertisementServiceImplementation.DeleteAsync(advertisementId);
        }
    }
}