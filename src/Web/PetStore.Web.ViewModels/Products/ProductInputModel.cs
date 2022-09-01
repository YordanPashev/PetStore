namespace PetStore.Web.ViewModels.Products
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PetStore.Data.Models;
    using PetStore.Data.Models.Common;
    using PetStore.Services.Mapping;

    public class ProductInputModel : IMapTo<Product>
    {
        [Required(ErrorMessage = ProductValidationConstants.NameIsRequired)]
        [MinLength(ProductValidationConstants.NameMinLength, ErrorMessage = ProductValidationConstants.NameMinLengthMessage)]
        [MaxLength(ProductValidationConstants.NameMaxLength, ErrorMessage = ProductValidationConstants.NameMaxLengthMessage)]
        public string Name { get; set; }

        [Range(ProductValidationConstants.PriceMinValue, ProductValidationConstants.PriceMaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string ImageURL { get; set; }

        public int CategoryId { get; set; }

        [Required(ErrorMessage = ProductValidationConstants.DescriptionIsRequired)]
        public string Description { get; set; }

        public DateTime CreatedOn => DateTime.UtcNow;
    }
}
