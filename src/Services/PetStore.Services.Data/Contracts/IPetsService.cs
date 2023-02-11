namespace PetStore.Services.Data.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;

    public interface IPetsService
    {
        Task AddPetAsync(Pet pet);

        IQueryable<Pet> GetAllPetsNoTracking();

        IQueryable<Pet> GetAllPetsForSelectedType(string typeName);

        Task<Pet> GetByIdAsync(string id);

        bool IsPetExistingInDb(Pet pet);
    }
}
