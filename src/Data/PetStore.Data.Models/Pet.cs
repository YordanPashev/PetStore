﻿// ReSharper disable RedundantNameQualifier
namespace PetStore.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PetStore.Data.Common.Models;
    using PetStore.Data.Models.Common;
    using PetStore.Data.Models.Enums;

    public class Pet : BaseDeletableModel<string>
    {
        public Pet()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(PetValidationConstants.NameMaxLength)]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [MaxLength(PetValidationConstants.BreedMaxLength)]
        public string Breed { get; set; }

        public PetGender Gender { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public PetType Type { get; set; }
    }
}
