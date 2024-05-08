using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsedCarHub.Domain;
using UsedCarHub.Domain.Entities;
using UsedCarHub.Repository.Interfaces;

namespace UsedCarHub.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public UserManager<UserEntity> UserManager { get; }
        public SignInManager<UserEntity> SignInManager { get; }

        public UnitOfWork(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager,
            AppDbContext dbContext)
        {
            _dbContext = dbContext;
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public async Task<bool> Commit()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                return false;
            }
        }
    }
}