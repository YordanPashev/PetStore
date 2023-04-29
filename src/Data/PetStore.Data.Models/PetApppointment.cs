namespace PetStore.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PetStore.Data.Common.Models;
    using PetStore.Data.Models.Common;

    public class PetApppointment : BaseDeletableModel<string>
    {
        public PetApppointment() => this.Id = Guid.NewGuid().ToString();

        [Key]
        public string Id { get; set; }

        public string ClientId { get; set; }

        [Required]
        [MaxLength(ClientValidationConstants.NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(ClientValidationConstants.NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(UserValidationConstants.PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime Appointment { get; set; }

        [Required]
        public string PetId { get; set; }

        [Required]
        public virtual Pet Pet { get; set; }
    }
}
