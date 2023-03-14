namespace PetStore.Web.ViewModels.User
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;
    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Address;
    using PetStore.Web.ViewModels.ClientCard;

    public class UserDetailsViewModel : IMapFrom<ApplicationUser>, IMapTo<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string AddressId { get; set; }

        public AdressDetailsViewModel Address { get; set; }

        public string ClientCardId { get; set; }

        public ClientCardDetaislsViewModel ClientCard { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
