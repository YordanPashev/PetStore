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
        private readonly IDeletableEntityRepository<Address> addressRepo;
        private readonly IDeletableEntityRepository<ClientCard> clientCardRepo;

        public UserService(IDeletableEntityRepository<ApplicationUser> userRepo, IDeletableEntityRepository<Address> adressRepo, IDeletableEntityRepository<ClientCard> clientCardRepo)
        {
            this.userRepo = userRepo;
            this.addressRepo = adressRepo;
            this.clientCardRepo = clientCardRepo;
        }

        public async Task DeactivateUserAccountAsync(ApplicationUser user)
        {
            this.userRepo.Delete(user);
            this.addressRepo.Delete(user.Address);
            this.clientCardRepo.Delete(user.ClientCard);

            await this.userRepo.SaveChangesAsync();
        }

        public async Task<UserDetailsViewModel> GetUserByIdAsycn(string userId)
        {
            ApplicationUser user = await this.userRepo
                                    .AllAsNoTracking()
                                    .Include(u => u.Address)
                                    .Include(u => u.ClientCard)
                                    .FirstOrDefaultAsync(u => u.Id == userId);

            return AutoMapperConfig.MapperInstance.Map<UserDetailsViewModel>(user);
        }

        public async Task<ApplicationUser> GetUserByIdForEditAsync(string id)
            => await this.userRepo.All()
                                  .Include(u => u.Address)
                                  .Include(u => u.ClientCard)
                                  .FirstOrDefaultAsync(u => u.Id == id);

        public async Task UpdateUserDataAsync(EditUserViewModel editModel, ApplicationUser user)
        {
            Address adress = await this.addressRepo.All().FirstOrDefaultAsync(a => a.Id == user.Address.Id);
            int townseparatorIndex = editModel.AddressText.IndexOf(",");
            string townName = editModel.AddressText.Substring(0, townseparatorIndex).Trim();
            adress.AddressText = editModel.AddressText;
            adress.TownName = townName;

            await this.addressRepo.SaveChangesAsync();

            user.FirstName = editModel.FirstName;
            user.LastName = editModel.LastName;
            user.Email = editModel.Email;
            user.PhoneNumber = editModel.PhoneNumber;

            await this.userRepo.SaveChangesAsync();
        }
    }
}
