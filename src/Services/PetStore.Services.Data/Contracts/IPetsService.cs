namespace PetStore.Services.Data.Contracts
{
    using System.Linq;

    using PetStore.Data.Models;

    public interface IPetsService
    {
        IQueryable<Pet> GetAllPetsNoTracking();
    }
}
