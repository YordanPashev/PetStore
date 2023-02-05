namespace PetStore.Web.ViewModels.Pets
{
    using System;

    using PetStore.Data.Models;
    using PetStore.Data.Models.Enums;
    using PetStore.Services.Mapping;

    public class PetViewModel : IMapFrom<Pet>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Age { get; set; }

        public string AgeInText => this.GetAgeInTextFormat();

        public string Breed { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public PetType Type { get; set; }

        public string UserMessage { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        private string GetAgeInTextFormat()
        {
            if (this.Age < 1)
            {
                int months = (int)(this.Age * 10);
                return $"{months} Months";
            }
            else
            {
                int years = (int)this.Age;
                string ageInString = this.Age.ToString();
                int months = int.Parse(ageInString[ageInString.Length - 1].ToString());

                if (months == 0)
                {
                    return $"{years} years";
                }
                else
                {
                    return $"{years} years and {months} months";
                }
            }
        }
    }
}
