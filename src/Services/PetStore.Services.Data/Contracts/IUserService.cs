namespace PetStore.Services.Data.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.User;

    public interface IUserService
    {
        Task DeactivateUserAccountAsync(ApplicationUser user);

        Task<UserDetailsViewModel> GetUserByIdAsycn(string userId);

        Task<ApplicationUser> GetUserByIdForEditAsync(string id);

        Task UpdateUserDataAsync(EditUserViewModel editModel, ApplicationUser user);
    }
}
