namespace PetStore.Web.ViewModels.User
{
    using PetStore.Data.Models;
    using PetStore.Services.Mapping;

    public class UserShortInfoViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
