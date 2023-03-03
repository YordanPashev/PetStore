namespace PetStore.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PetStore.Web.ViewModels.User;

    public interface IUserService
    {
        Task<UserViewModel> GetClientByIdAsycn(string userId);
    }
}
