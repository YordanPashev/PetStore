namespace PetStore.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
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

        public bool IsPetExistingInDb(string petName)
            => this.petsRepo
                   .AllAsNoTracking()
                   .Any(p => p.Name == petName);
    }
}
