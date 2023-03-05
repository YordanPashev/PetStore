namespace PetStore.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.User;

    public interface IUserService
    {
        Task DeactivateUserAccountAsync(ApplicationUser user);

        Task<UserDetailsViewModel> GetClientByIdAsycn(string userId);

        Task<ApplicationUser> GetUserByIdForEditAsync(string id);
    }
}
