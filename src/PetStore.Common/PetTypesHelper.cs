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
            string monthsWord = months == 1 ? "month" : "months";

            if (years == 0)
            {
                return $"{months} {monthsWord}";
            }

            string yearsWord = years == 1 ? "year" : "years";

            if (months == 0)
            {
                return $"{years} {yearsWord}";
            }

            return $"{years} {yearsWord} and {months} {monthsWord}";
        }
    }
}
