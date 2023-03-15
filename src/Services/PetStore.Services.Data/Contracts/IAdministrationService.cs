﻿namespace PetStore.Services.Data.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;

    public interface IAdministrationService
    {
        Task ActivateUserAccount(ApplicationUser user);

        Task<ApplicationUser> GetInctiveUserByIdForEditAsync(string id);

        IQueryable<ApplicationUser> GetAllUsersWithDeleted();
    }
}
