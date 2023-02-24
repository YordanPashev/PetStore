namespace PetStore.Web.ViewModels.Client
{
    using System;

    using PetStore.Data.Models;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.ClientCard;

    public class UserViewModel : IMapFrom<ApplicationUser>, IMapTo<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AddressId { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ClientCardDetaislsViewModel ClientCard { get; set; }
    }
}
