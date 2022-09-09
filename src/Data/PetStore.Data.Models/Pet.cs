// ReSharper disable RedundantNameQualifier
namespace PetStore.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using PetStore.Data.Common.Models;
    using PetStore.Data.Models.Common;
    using PetStore.Data.Models.Enums;

    public class Pet : BaseDeletableModel<string>
    {
        public Pet()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [MaxLength(PetValidationConstants.NameMaxLength)]
        public string Name { get; set; }

        public int Age { get; set; }

        [Required]
        public string Breed { get; set; }

        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public PetType Type { get; set; }

        [Required]
        [ForeignKey(nameof(Client))]
        public string OwnerId { get; set; }

        public virtual Client Client { get; set; }

        [Required]
        [ForeignKey(nameof(Store))]
        public string StoreId { get; set; }

        public virtual Store Store { get; set; }
    }
}
