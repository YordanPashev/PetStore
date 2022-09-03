namespace PetStore.Data.Models.Common
{
    public static class ProductValidationConstants
    {
        //Message
        public const string NameIsRequired = "Product name is required!";
        public const string NameMinLengthMessage = "The name can not be less than 2 characters";
        public const string NameMaxLengthMessage = "The name can not be more than 70 characters long";
        public const string DescriptionIsRequired = "Description is required!";
        public const string InvalidUrlMessage = "The image URL is not valid!";

        public const int NameMinLength = 2;
        public const int NameMaxLength = 70;

        public const int DescriptionMinLength = 10;
        public const int DescriptionMaxLength = 4000;

        public const double PriceMinValue = 0;
        public const double PriceMaxValue = 1_000_000;

        public const string UrlRegex = @"^(https://)(.)+";
        public const string UrlRegexForView = "^(https://)(.)+";

    }
}
