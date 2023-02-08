namespace PetStore.Services.Data.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.Pets;

    public interface IPetsService
    {
        IQueryable<Pet> GetAllPetsNoTracking();

        bool IsPetExistingInDb(Pet pet);

        Task AddPetAsync(Pet pet);

        Task<Pet> GetByIdAsync(string id);

        IQueryable<Pet> GetAllPetsWithSelectedType(string typeName);
    }
}
