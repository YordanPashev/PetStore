namespace PetStore.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using PetStore.Data.Models;
    using PetStore.Data.Models.Common;
    using PetStore.Services.Mapping;

    public class InputCategoryViewModel : IMapTo<Category>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = CategoryValidationConstants.NameIsRequired)]
        [MinLength(CategoryValidationConstants.NameMinLength)]
        [MaxLength(CategoryValidationConstants.NameMaxLength)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression(CategoryValidationConstants.UrlRegex, ErrorMessage = CategoryValidationConstants.InvalidUrlMessage)]
        public string ImageURL { get; set; }
    }
}
