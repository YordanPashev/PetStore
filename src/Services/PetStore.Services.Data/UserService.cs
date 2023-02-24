namespace PetStore.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;

    public class UserService : IUserService
    {

        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;

        public UserService(IDeletableEntityRepository<ApplicationUser> userRepo)
            => this.userRepo = userRepo;

        public async Task<ApplicationUser> GetClientByIdAsycn(string userId)
            => await this.userRepo
                    .AllAsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == userId);
    }
}
