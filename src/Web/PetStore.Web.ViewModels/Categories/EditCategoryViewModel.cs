namespace PetStore.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using PetStore.Data.Models;
    using PetStore.Data.Models.Common;
    using PetStore.Services.Mapping;

    public class EditCategoryViewModel : IMapTo<Category>
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = CategoryValidationConstants.NameIsRequired)]
        [MinLength(CategoryValidationConstants.NameMinLength)]
        [MaxLength(CategoryValidationConstants.NameMaxLength)]
        public string Name { get; set; }

        [RegularExpression(CategoryValidationConstants.UrlRegex, ErrorMessage = CategoryValidationConstants.InvalidUrlMessage)]
        public string ImageUrl { get; set; }
    }
}
