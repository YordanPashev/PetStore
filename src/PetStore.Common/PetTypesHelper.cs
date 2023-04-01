namespace PetStore.Common
{
    using System;
    using System.Text;

    public static class PetTypesHelper
    {
        public static string GetPetTypePlural(string petType)
        {
            StringBuilder petTypePlural = new StringBuilder(petType);
            petTypePlural = petTypePlural.ToString() == "Fish" ? petTypePlural : petTypePlural.Append("s");

            return petTypePlural.ToString();
        }

        public static string FormatAgeToText(DateTime birthDate)
        {
            var spawnResult = SpawnTimeCalculator.CompareDates(DateTime.Now, birthDate);
            int years = spawnResult.Years;
            int months = spawnResult.Months;

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
