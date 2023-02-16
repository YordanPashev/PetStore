namespace PetStore.Services.Data.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Data.Models.Enums;
    using PetStore.Web.ViewModels.Products;

    public interface IPetsService
    {
        Task AddPetAsync(Pet pet);

        Task DeletePetAsync(Pet pet);

        IQueryable<Pet> GetAllDeletedPetsNoTracking();

        IQueryable<Pet> GetAllPetsInSaleNoTracking();

        IQueryable<Pet> GetAllPetsInSaleForSelectedType(string typeName);

        Task<Pet> GetDeletedPetByIdAsync(string id);

        Task<Pet> GetDeletedPetByIdAsyncNoTracking(string id);

        Task<Pet> GetPetByIdAsync(string id);

        Task<Pet> GetPetByIdForEditAsync(string id);

        bool IsPetExistingInDb(Pet pet);

        Task UpdatePetDataAsync(EditPetViewModel userInputModel, Pet pet, PetType petType);

        Task UndeleteProductAsync(Pet product);
    }
}
