namespace PetStore.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using PetStore.Data.Common.Models;
    using PetStore.Data.Models.Common;

    public class Order : BaseDeletableModel<string>
    {
        public Order() => this.Id = Guid.NewGuid().ToString();

        [Key]
        public string Id { get; set; }

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
        [MaxLength(AddressValidationConstants.TextMaxLength)]
        public string Address { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey(nameof(Product))]
        public string ProductId { get; set; }

        public Product Product { get; set; }
    }
}
