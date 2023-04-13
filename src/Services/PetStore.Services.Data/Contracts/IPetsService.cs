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

        IQueryable<Pet> GetAllRemovedPets(string orderByCriteria);

        IQueryable<Pet> GetAllPetsInSale(string orderByCriteria);

        IQueryable<Pet> GetAllSearchedPetsInSale(string searchQuery, string orderByCriteria);

        IQueryable<Pet> GetAllTypePetsInSale(string typeName, string orderByCriteria);

        IQueryable<Pet> GetAllSearchedTypePetsInSale(string typeName, string searchQuery, string orderByCriteria);

        IQueryable<Pet> GetAllSearchedRemovedPets(string searchQuery, string orderByCriteria);

        Task<Pet> GetDeletedPetByIdAsync(string id);

        Task<Pet> GetDeletedPetByIdAsyncNoTracking(string id);

        Task<Pet> GetPetByIdAsync(string id);

        Task<Pet> GetPetByIdForEditAsync(string id);

        bool IsPetExistingInDb(Pet pet);

        Task UpdatePetDataAsync(EditPetViewModel userInputModel, Pet pet);

        Task UndeletePetAsync(Pet product);
    }
}
