namespace PetStore.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PetStore.Common;
    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Data.Models.Enums;
    using PetStore.Services.Data.Contracts;
    using PetStore.Web.ViewModels.Products;

    public class PetsService : IPetsService
    {
        private readonly IDeletableEntityRepository<Pet> petsRepo;

        public PetsService(IDeletableEntityRepository<Pet> petsRepo)
            => this.petsRepo = petsRepo;

        public async Task AddPetAsync(Pet pet)
        {
            pet.Price = Math.Round(pet.Price, 2);

            await this.petsRepo.AddAsync(pet);
            await this.petsRepo.SaveChangesAsync();
        }

        public async Task DeletePetAsync(Pet pet)
        {
            this.petsRepo.Delete(pet);
            await this.petsRepo.SaveChangesAsync();
        }

        public IQueryable<Pet> GetAllRemovedPets(string orderCriteria)
        {
            IQueryable<Pet> result = this.petsRepo.AllAsNoTrackingWithDeleted()
                                                  .Where(p => p.IsDeleted)
                                                  .OrderBy(p => p.Name);
            result = this.OrderByCriteria(orderCriteria, result);

            return result;
        }

        public IQueryable<Pet> GetAllPetsInSale(string orderCriteria)
        {
            IQueryable<Pet> result = this.petsRepo.AllAsNoTracking();
            result = this.OrderByCriteria(orderCriteria, result);

            return result;
        }

        public IQueryable<Pet> GetAllTypePetsInSale(string typeName, string orderCriteria)
        {
            PetType petType;

            if (Enum.TryParse<PetType>(typeName, out petType))
            {
                IQueryable<Pet> result = this.petsRepo.AllAsNoTracking()
                                                      .Where(p => p.Type == petType)
                                                      .OrderBy(c => c.Name);
                result = this.OrderByCriteria(orderCriteria, result);

                return result;
            }

            return Enumerable.Empty<Pet>().AsQueryable();
        }

        public IQueryable<Pet> GetAllSearchedPetsInSale(string searchQueryCapitalCase, string orderCriteria)
        {
            IQueryable<Pet> result = this.petsRepo.AllAsNoTracking()
                                                  .Where(p => p.Name.ToUpper().Contains(searchQueryCapitalCase))
                                                  .OrderBy(p => p.Name);
            result = this.OrderByCriteria(orderCriteria, result);

            return result;
        }

        public IQueryable<Pet> GetAllSearchedTypePetsInSale(string typeName, string searchQueryCapitalCase, string orderCriteria)
        {
            PetType petType;

            if (Enum.TryParse<PetType>(typeName, out petType))
            {
                IQueryable<Pet> result = this.petsRepo.AllAsNoTracking()
                                                      .Where(p => p.Type == petType)
                                                      .Where(p => p.Name.ToUpper().Contains(searchQueryCapitalCase))
                                                      .OrderBy(c => c.Name);
                result = this.OrderByCriteria(orderCriteria, result);

                return result;
            }

            return Enumerable.Empty<Pet>().AsQueryable();
        }

        public IQueryable<Pet> GetAllSearchedRemovedPets(string searchQueryCapitalCase, string orderCriteria)
        {
            IQueryable<Pet> result = this.petsRepo.AllAsNoTrackingWithDeleted()
                                                  .Where(p => p.IsDeleted)
                                                  .Where(p => p.Name.ToUpper().Contains(searchQueryCapitalCase))
                                                  .OrderBy(p => p.Name);
            result = this.OrderByCriteria(orderCriteria, result);

            return result;
        }

        public async Task<Pet> GetDeletedPetByIdAsync(string id)
            => await this.petsRepo
                    .AllWithDeleted()
                    .Where(p => p.IsDeleted)
                    .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Pet> GetDeletedPetByIdAsyncNoTracking(string id)
            => await this.petsRepo
                    .AllAsNoTrackingWithDeleted()
                    .Where(p => p.IsDeleted)
                    .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Pet> GetPetByIdAsync(string id)
            => await this.petsRepo
                    .AllAsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Pet> GetPetByIdForEditAsync(string id)
            => await this.petsRepo
                    .All()
                    .FirstOrDefaultAsync(p => p.Id == id);

        public bool IsPetExistingInDb(Pet pet)
            => this.petsRepo
                   .AllAsNoTracking()
                   .Any(p => p.Name == pet.Name && p.Type == pet.Type &&
                        p.Breed == pet.Breed && p.BirthDate == pet.BirthDate);

        public async Task UpdatePetDataAsync(EditPetViewModel userInputModel, Pet pet, PetType petType)
        {
            pet.Name = userInputModel.Name;
            pet.Type = petType;
            pet.Breed = userInputModel.Breed;
            pet.BirthDate = userInputModel.BirthDate;
            pet.Price = Math.Round(userInputModel.Price, 2);
            pet.ImageUrl = userInputModel.ImageUrl;

            await this.petsRepo.SaveChangesAsync();
        }

        public async Task UndeletePetAsync(Pet pet)
        {
            pet.DeletedOn = null;
            pet.IsDeleted = false;
            await this.petsRepo.SaveChangesAsync();
        }

        private IQueryable<Pet> OrderByCriteria(string orderCriteria, IQueryable<Pet> result)
        {
            if (orderCriteria == GlobalConstants.CriteriaPriceAscending)
            {
                return result.OrderBy(p => p.Price)
                             .ThenBy(p => p.Name);
            }
            else if (orderCriteria == GlobalConstants.CriteriaPriceDescending)
            {
                return result.OrderByDescending(p => p.Price)
                             .ThenBy(p => p.Name);
            }
            else if (orderCriteria == GlobalConstants.CriteriaType)
            {
                return result.OrderBy(p => p.Type)
                             .ThenBy(p => p.Name);
            }
            else if (orderCriteria == GlobalConstants.CriteriaRiecent)
            {
                return result.OrderByDescending(p => p.CreatedOn)
                             .ThenBy(p => p.Name);
            }

            return result.OrderBy(p => p.Name);
        }
    }
}
