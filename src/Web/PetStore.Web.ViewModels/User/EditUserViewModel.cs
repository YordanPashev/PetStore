namespace PetStore.Web.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Data.Models.Common;
    using PetStore.Services.Mapping;

    public class EditUserViewModel : IMapFrom<ApplicationUser>, IMapTo<ApplicationUser>
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [MinLength(UserValidationConstants.NameMinLength, ErrorMessage = GlobalConstants.UserNameMinLengthMessage)]
        [MaxLength(UserValidationConstants.NameMaxLength, ErrorMessage = GlobalConstants.UserNameMaxLengthMessage)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(UserValidationConstants.NameMinLength, ErrorMessage = GlobalConstants.UserNameMinLengthMessage)]
        [MaxLength(UserValidationConstants.NameMaxLength, ErrorMessage = GlobalConstants.UserNameMaxLengthMessage)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = GlobalConstants.UserEmailErrorMessage)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(UserValidationConstants.PhoneNumberRegex, ErrorMessage = GlobalConstants.UserPhoneNumberMessage)]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(UserValidationConstants.DeliveryAddressRegex, ErrorMessage = GlobalConstants.UserDeliveryAddressMessage)]
        public string AddressText { get; set; }

        public string UserMessage { get; set; }
    }
}
