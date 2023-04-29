namespace PetStore.Web.ViewModels.Appointment
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Data.Models.Common;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Pets;

    public class MakeAnPetAppointmentViewModel : IMapTo<PetApppointment>
    {
        public string ClientId { get; set; }

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
        public DateTime Appointment { get; set; }

        [Required]
        public string PetId { get; set; }

        public PetDetailsViewModel Pet { get; set; }
    }
}
