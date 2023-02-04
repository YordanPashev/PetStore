namespace PetStore.Services.Data
{
    using System.Linq;
    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;

    public class PetsService : IPetsService
    {
        private readonly IDeletableEntityRepository<Pet> petsRepo;

        public PetsService(IDeletableEntityRepository<Pet> petsRepo)
            => this.petsRepo = petsRepo;

        public IQueryable<Pet> GetAllPetsNoTracking()
            => this.petsRepo.AllAsNoTracking()
                     .OrderBy(c => c.Name);
    }
}
