namespace PetStore.Web.ViewModels.Contact
{
    using System.ComponentModel.DataAnnotations;

    using PetStore.Data.Models.Common;

    public class RequestViewModel
    {
        public string Id { get; set; }

        [Required]
        [MinLength(RequestValidationConstants.TitleMinLength)]
        [MaxLength(RequestValidationConstants.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(RequestValidationConstants.MessageMinLength)]
        [MaxLength(RequestValidationConstants.MessageMaxLength)]
        public string Message { get; set; }

        [Required]
        [EmailAddress]
        public string SenderEmail { get; set; }
    }
}
