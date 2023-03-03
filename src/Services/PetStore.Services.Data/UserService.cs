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

        public UserService(IDeletableEntityRepository<ApplicationUser> userRepo)
            => this.userRepo = userRepo;

        public async Task<UserViewModel> GetClientByIdAsycn(string userId)
        {
            ApplicationUser user = await this.userRepo
                                    .AllAsNoTracking()
                                    .Include(u => u.Address)
                                    .Include(u => u.ClientCard)
                                    .FirstOrDefaultAsync(u => u.Id == userId);

            return AutoMapperConfig.MapperInstance.Map<UserViewModel>(user);
        }
    }
}
