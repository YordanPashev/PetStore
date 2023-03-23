namespace PetStore.Web.ViewModels.Requests
{
    using System.ComponentModel.DataAnnotations;

    using PetStore.Data.Models;
    using PetStore.Data.Models.Common;
    using PetStore.Services.Mapping;

    public class CreateRequestViewModel : IMapFrom<Request>, IMapTo<Request>
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
