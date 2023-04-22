namespace PetStore.Web.ViewModels.Orders
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Data.Models.Common;
    using PetStore.Web.ViewModels.Products;

    public class OrderDetailsViewModel
    {
        [Required]
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
        [RegularExpression(UserValidationConstants.DeliveryAddressRegex, ErrorMessage = GlobalConstants.UserDeliveryAddressMessage)]
        public string Address { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ProductImageUrl { get; set; }

        [Required]
        public string ProductCategoryName { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        [Required]
        public int? ClientCardDiscount { get; set; }

        public decimal TotalPriceWithoutDiscount => this.ProductPrice * this.Quantity;

        public decimal? TotalPriceWithDiscount => this.GetTotalPriceWithDiscount();

        public string UserErrorMessage { get; set; }

        private decimal? GetTotalPriceWithDiscount()
        {
            if (this.ClientId == null || this.ClientCardDiscount == null)
            {
                return null;
            }

            decimal discount = ((decimal)this.ClientCardDiscount / 100) * this.TotalPriceWithoutDiscount;
            decimal totalPriceWithDiscount = this.TotalPriceWithoutDiscount - discount;

            return Math.Round(totalPriceWithDiscount, 2);
        }
    }
}
