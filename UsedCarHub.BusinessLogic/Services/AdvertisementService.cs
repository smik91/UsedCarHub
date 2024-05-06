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
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        public AdvertisementService(IUnitOfWork unitOfWork, IAdvertisementRepository advertisementRepository,ICarRepository carRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _advertisementRepository = advertisementRepository;
            _carRepository = carRepository;
            _mapper = mapper;
        }
        
        public async Task<Result<AdvertisementDto>> AddAsync(AddAdvertisementDto addAdvertisementDto)
        {
            CarEntity car = _mapper.Map<CarEntity>(addAdvertisementDto.Car);
            var carAddResult = await _carRepository.AddAsync(car);
            if (!carAddResult.IsSuccess)
                return Result<AdvertisementDto>.Failure(carAddResult.ExecutionErrors);
            var advertisement = _mapper.Map<AdvertisementEntity>(addAdvertisementDto);
            advertisement.CarId = carAddResult.Value.Id;
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

        public Task<Result<UpdateAdvertisementDto>> UpdateAsync(string advertisementId, UpdateAdvertisementDto updateUserDto)
        {
            return _advertisementServiceImplementation.UpdateAsync(advertisementId, updateUserDto);
        }

        public Task<Result<AdvertisementInfoDto>> GetInfoAsync(string advertisementId)
        {
            return _advertisementServiceImplementation.GetInfoAsync(advertisementId);
        }

        public Task<Result<string>> DeleteAsync(string advertisementId)
        {
            return _advertisementServiceImplementation.DeleteAsync(advertisementId);
        }
    }
}