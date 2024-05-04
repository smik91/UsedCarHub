using Microsoft.AspNetCore.Identity;
using UsedCarHub.Domain.Entities;

namespace UsedCarHub.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        UserManager<UserEntity> UserManager { get; }
        SignInManager<UserEntity> SignInManager { get; }
        Task<bool> Commit();
    }
}