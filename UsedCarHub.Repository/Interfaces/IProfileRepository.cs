using UsedCarHub.Common.Results;
using UsedCarHub.Domain.Entities;

namespace UsedCarHub.Repository.Interfaces
{
    public interface IProfileRepository
    {
        public Task<Result<ProfileEntity>> GetAsync(int profileId);
        public Task<Result<ProfileEntity>> DeleteAsync(int profileId);
        public Task<Result<ProfileEntity>> AddAsync(ProfileEntity profile);
        public Task<Result<ProfileEntity>> UpdateAsync(int profileId, ProfileEntity profile);
    }
}