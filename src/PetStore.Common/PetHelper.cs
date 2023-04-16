namespace PetStore.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class PetHelper
    {
        public static string FormatAgeToText(DateTime birthDate)
        {
            var spawnResult = SpawnTimeCalculator.CompareDates(DateTime.Now, birthDate);
            int numberOfYears = spawnResult.Years;
            int numberOfMonths = spawnResult.Months;
            string monthsForm = numberOfMonths == 1 ? "month" : "months";

            if (numberOfYears == 0)
            {
                return $"{numberOfMonths} {monthsForm}";
            }

            string yearsForm = numberOfYears == 1 ? "year" : "years";

            if (numberOfMonths == 0)
            {
                return $"{numberOfYears} {yearsForm}";
            }

            return $"{numberOfYears} {yearsForm} and {numberOfMonths} {monthsForm}";
        }

        public static string GetPetTypePlural(string petType)
        {
            StringBuilder petTypePlural = new StringBuilder(petType);
            petTypePlural = petTypePlural.ToString() == "Fish" ? petTypePlural : petTypePlural.Append('s');

            return petTypePlural.ToString();
        }
    }
}
