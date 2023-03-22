namespace PetStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;

    public class AdministrationsService : IAdministrationsService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Address> addressRepo;
        private readonly IDeletableEntityRepository<ClientCard> clientCardRepo;

        public AdministrationsService(IDeletableEntityRepository<ApplicationUser> userRepo, IDeletableEntityRepository<Address> adressRepo, IDeletableEntityRepository<ClientCard> clientCardRepo)
        {
            this.userRepo = userRepo;
            this.addressRepo = adressRepo;
            this.clientCardRepo = clientCardRepo;
        }

        public async Task ActivateUserAccount(ApplicationUser user)
        {
            Address address = await this.addressRepo.AllWithDeleted()
                                                    .FirstOrDefaultAsync(u => u.ClientId == user.Id);
            ClientCard clientCard = await this.clientCardRepo.AllWithDeleted()
                                                    .FirstOrDefaultAsync(u => u.ClientId == user.Id);
            user.IsDeleted = false;
            user.DeletedOn = null;
            address.IsDeleted = false;
            address.DeletedOn = null;
            clientCard.IsDeleted = false;
            clientCard.DeletedOn = null;

            await this.addressRepo.SaveChangesAsync();
            await this.clientCardRepo.SaveChangesAsync();
            await this.userRepo.SaveChangesAsync();
        }

        public IQueryable<ApplicationUser> GetAllUsersWithDeleted()
            => this.userRepo.AllAsNoTrackingWithDeleted()
                            .Where(u => u.Roles.Count == 0)
                            .OrderBy(u => u.Email);

        public async Task<ApplicationUser> GetInctiveUserByIdForEditAsync(string id)
            => await this.userRepo.AllWithDeleted()
                                  .Where(u => u.IsDeleted == true)
                                  .Include(u => u.Address)
                                  .Include(u => u.ClientCard)
                                  .FirstOrDefaultAsync(u => u.Id == id);
    }
}
