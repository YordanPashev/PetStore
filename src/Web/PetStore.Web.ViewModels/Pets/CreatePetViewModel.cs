namespace PetStore.Web.ViewModels.Pets
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PetStore.Data.Models.Common;

    public class CreatePetViewModel
    {
        [Required]
        [MinLength(PetValidationConstants.NameMinLength)]
        [MaxLength(PetValidationConstants.NameMaxLength)]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [MinLength(PetValidationConstants.BreedMinLength)]
        [MaxLength(PetValidationConstants.BreedMaxLength)]
        public string Breed { get; set; }

        [Required]
        [Range(PetValidationConstants.PriceMinValue, PetValidationConstants.PriceMaxValue)]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression(ProductValidationConstants.UrlRegex, ErrorMessage = ProductValidationConstants.InvalidUrlMessage)]
        public string ImageUrl { get; set; }

        [Required]
        public string TypeName { get; set; }
    }
}
