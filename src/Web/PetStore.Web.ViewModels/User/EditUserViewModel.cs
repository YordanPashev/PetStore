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
        [MinLength(UserValidationConstants.NameMinLength, ErrorMessage = GlobalConstants.UserNameMinLengthErrormessage)]
        [MaxLength(UserValidationConstants.NameMaxLength, ErrorMessage = GlobalConstants.UserNameMaxLengthErrormessage)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(UserValidationConstants.NameMinLength, ErrorMessage = GlobalConstants.UserNameMinLengthErrormessage)]
        [MaxLength(UserValidationConstants.NameMaxLength, ErrorMessage = GlobalConstants.UserNameMaxLengthErrormessage)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = GlobalConstants.UserEmailErrorMessage)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(UserValidationConstants.PhoneNumberRrgex, ErrorMessage = GlobalConstants.UserPhoneNumberErrormessage)]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(UserValidationConstants.DeliveryAddressRrgex, ErrorMessage = GlobalConstants.UserDeliveryAddressErrormessage)]
        public string AddressText { get; set; }

        public string UserMessage { get; set; }
    }
}
