// ReSharper disable InconsistentNaming
namespace PetStore.Data.Models.Common
{
    public static class PetValidationConstants
    {
        public const int NameMinLength = 2;
        public const int NameMaxLength = 50;

        public const double AgeMinLength = 0.1;
        public const double AgeMaxLength = 300;

        public const int BreedMinLength = 2;
        public const int BreedMaxLength = 50;

        public const double PriceMinValue = 0.1;
        public const double PriceMaxValue = 1_000_000;

        public const string UrlRegex = @"^(https://)(.)+";
        public const string UrlRegexForView = "^(https://)(.)+";
    }
}