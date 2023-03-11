namespace PetStore.Services.Data.Contracts
{
    using System.Linq;

    using PetStore.Data.Models;

    public interface IAdministrationService
    {
        IQueryable<ApplicationUser> GetAllUsers();
    }
}
