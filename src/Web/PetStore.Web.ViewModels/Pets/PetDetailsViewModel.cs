﻿namespace PetStore.Web.ViewModels.Pets
{
    using System;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Data.Models.Enums;
    using PetStore.Services.Mapping;

    public class PetDetailsViewModel : IMapFrom<Pet>, IMapTo<Pet>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int AgeMonts { get; set; }

        public string AgeInTextFormat => PetHelper.FormatAgeToText(this.BirthDate);

        public string Breed { get; set; }

        public PetGender Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public PetType Type { get; set; }

        public string TypeName => this.Type.ToString();

        public string GenderInTextFormat => this.Gender.ToString();

        public string TypePlural => PetHelper.GetPetTypePlural(this.TypeName);

        public string UserMessage { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string UserHasAppointmentForThisPetMessage { get; set; }
    }
}
