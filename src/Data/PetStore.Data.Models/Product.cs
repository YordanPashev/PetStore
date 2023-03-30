// ReSharper disable RedundantNameQualifier
namespace PetStore.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using PetStore.Data.Common.Models;
    using PetStore.Data.Models.Common;

    public class Product : BaseDeletableModel<string>
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(ProductValidationConstants.NameMaxLength)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
