namespace PetStore.Services.Data
{
    using System.Linq;

    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;

    public class AdministrationService : IAdministrationService
    {

        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Address> addressRepo;
        private readonly IDeletableEntityRepository<ClientCard> clientCardRepo;

        public AdministrationService(IDeletableEntityRepository<ApplicationUser> userRepo, IDeletableEntityRepository<Address> adressRepo, IDeletableEntityRepository<ClientCard> clientCardRepo)
        {
            this.userRepo = userRepo;
            this.addressRepo = adressRepo;
            this.clientCardRepo = clientCardRepo;
        }

        public IQueryable<ApplicationUser> GetAllUsers()
            => this.userRepo.AllAsNoTrackingWithDeleted()
                            .Where(x => x.Roles.Count == 0);
    }
}
