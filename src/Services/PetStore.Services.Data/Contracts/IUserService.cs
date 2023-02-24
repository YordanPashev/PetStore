﻿namespace PetStore.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PetStore.Data.Models;

    public interface IUserService
    {
        Task<ApplicationUser> GetClientByIdAsycn(string userId);
    }
}
