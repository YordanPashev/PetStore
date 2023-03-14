namespace PetStore.Web.ViewModels.User
{
    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Mapping;

    public class UserShortInfoViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string AccountStatus => this.GetAccountStatus();

        public bool IsDeleted { get; set; }

        private string GetAccountStatus()
            => this.IsDeleted == false ? GlobalConstants.AccountStatusActive
                                       : GlobalConstants.AccountStatusInactive;
    }
}
