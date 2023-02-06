﻿namespace PetStore.Web.ViewModels.Pets
{
    using System;

    using PetStore.Data.Models;
    using PetStore.Data.Models.Enums;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Common;

    public class PetViewModel : IMapFrom<Pet>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int AgeMonts { get; set; }

        public string AgeInTextFormat => this.FormatAgeToText();

        public string Breed { get; set; }

        public DateTime BirthDate { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public PetType Type { get; set; }

        public string UserMessage { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        private string FormatAgeToText()
        {
            var dateSpan = DateTimeSpan.CompareDates(DateTime.Now, this.BirthDate);
            int years = dateSpan.Years;
            int months = dateSpan.Months;

            if (years == 0)
            {
                return $"{months} months";
            }
            else if (months == 0)
            {
                return $"{years} years";
            }

            return $"{years} years and {months} months";
        }
    }
}
