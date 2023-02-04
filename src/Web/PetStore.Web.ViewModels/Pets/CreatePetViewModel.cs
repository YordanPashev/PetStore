namespace PetStore.Web.ViewModels.Pets
{
    using System.ComponentModel.DataAnnotations;

    using PetStore.Data.Models.Common;
    using PetStore.Data.Models.Enums;

    public class CreatePetViewModel
    {
        [Required]
        [MinLength(PetValidationConstants.NameMinLength)]
        [MaxLength(PetValidationConstants.NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [Range(PetValidationConstants.AgeMinLength, PetValidationConstants.AgeMaxLength)]
        public double Age { get; set; }

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
        public string Type { get; set; }
    }
}
