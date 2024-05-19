using Microsoft.EntityFrameworkCore;
using UsedCarHub.Common.Errors;
using UsedCarHub.Common.Results;
using UsedCarHub.Domain;
using UsedCarHub.Domain.Entities;
using UsedCarHub.Repository.Interfaces;

namespace UsedCarHub.Repository.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext _dbContext;

        public ProfileRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Result<ProfileEntity>> GetAsync(int profileId)
        {
            var profile = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.Id == profileId);
            if (profile == null)
            {
                return Result<ProfileEntity>.Failure(ProfileError.NotFoundById);
            }

            return Result<ProfileEntity>.Success(profile);
        }

        public async Task<Result<ProfileEntity>> DeleteAsync(int profileId)
        {
            var profile = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.Id == profileId);
            if (profile == null)
            {
                return Result<ProfileEntity>.Failure(ProfileError.NotFoundById);
            }

            _dbContext.Profiles.Remove(profile);
            await _dbContext.SaveChangesAsync();
            return Result<ProfileEntity>.Success(profile);
        }

        public async Task<Result<ProfileEntity>> AddAsync(ProfileEntity profile)
        {
            await _dbContext.Profiles.AddAsync(profile);
            await _dbContext.SaveChangesAsync();
            return Result<ProfileEntity>.Success(profile);
        }

        public async Task<Result<ProfileEntity>> UpdateAsync(int profileId, ProfileEntity profile)
        {
            var profileGet = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.Id == profileId);
            if (profileGet == null)
            {
                return Result<ProfileEntity>.Failure(ProfileError.NotFoundById);
            }

            profileGet.FirstName = profile.FirstName;
            profileGet.LastName = profile.LastName;
            profileGet.AvatarUrl = profile.AvatarUrl;
            await _dbContext.SaveChangesAsync();
            return Result<ProfileEntity>.Success(profileGet);
        }
    }
}