namespace PetStore.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.User;

    public interface IUsersService
    {
        Task DeactivateUserAccountAsync(ApplicationUser user);

        Task<UserDetailsViewModel> GetActiveUserByIdAsycn(string userId);

        Task<UserDetailsViewModel> GetUserByIdWtihDeactivatedAsycn(string userId);

        Task<ApplicationUser> GetActiveUserByIdForEditAsync(string id);

        Task UpdateUserDataAsync(EditUserViewModel editModel, ApplicationUser user);
    }
}
