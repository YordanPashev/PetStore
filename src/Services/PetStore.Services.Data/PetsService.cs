namespace PetStore.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;

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

        public IQueryable<Pet> GetAllPetsNoTracking()
            => this.petsRepo.AllAsNoTracking()
                     .OrderBy(c => c.Name);

        public async Task<Pet> GetByIdAsync(string id)
            => await this.petsRepo
                    .AllAsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);

        public bool IsPetExistingInDb(Pet pet)
            => this.petsRepo
                   .AllAsNoTracking()
                   .Any(p => p.Name == pet.Name && p.Type == pet.Type &&
                        p.Breed == pet.Breed && p.Age == pet.Age &&
                        p.Price == pet.Price && p.ImageUrl == pet.ImageUrl);
    }
}
