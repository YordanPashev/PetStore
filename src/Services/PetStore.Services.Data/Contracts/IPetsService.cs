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

        IQueryable<Pet> GetAllPetsNoTracking();

        IQueryable<Pet> GetAllPetsForSelectedType(string typeName);

        Task<Pet> GetByIdAsync(string id);

        Task<Pet> GetPetByIdForEditAsync(string id);

        bool IsPetExistingInDb(Pet pet);

        Task UpdatePetDataAsync(EditPetViewModel userInputModel, Pet pet, PetType petType);
    }
}
