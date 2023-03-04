namespace PetStore.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.User;

    public class UserService : IUserService
    {

        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Address> adressRepo;
        private readonly IDeletableEntityRepository<ClientCard> clientCardRepo;


        public UserService(IDeletableEntityRepository<ApplicationUser> userRepo, IDeletableEntityRepository<Address> adressRepo, IDeletableEntityRepository<ClientCard> clientCardRepo)
        {
            this.userRepo = userRepo;
            this.adressRepo = adressRepo;
            this.clientCardRepo = clientCardRepo;
        }

        public async Task DeactivateUserAccountAsync(ApplicationUser user)
        {
            this.userRepo.Delete(user);
            this.adressRepo.Delete(user.Address);
            this.clientCardRepo.Delete(user.ClientCard);

            await this.userRepo.SaveChangesAsync();
        }

        public async Task<UserViewModel> GetClientByIdAsycn(string userId)
        {
            ApplicationUser user = await this.userRepo
                                    .AllAsNoTracking()
                                    .Include(u => u.Address)
                                    .Include(u => u.ClientCard)
                                    .FirstOrDefaultAsync(u => u.Id == userId);

            return AutoMapperConfig.MapperInstance.Map<UserViewModel>(user);
        }

        public async Task<ApplicationUser> GetUserByIdForEditAsync(string id)
            => await this.userRepo.All()
                                  .Include(u => u.Address)
                                  .Include(u => u.ClientCard)
                                  .FirstOrDefaultAsync(u => u.Id == id);
    }
}
