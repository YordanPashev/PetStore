namespace PetStore.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PetStore.Data.Common.Models;
    using PetStore.Data.Models.Common;

    public class Request : BaseDeletableModel<string>
    {
        public Request()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(RequestValidationConstants.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(RequestValidationConstants.MessageMaxLength)]
        public string Message { get; set; }

        [Required]
        public string SenderEmail { get; set; }
    }
}
