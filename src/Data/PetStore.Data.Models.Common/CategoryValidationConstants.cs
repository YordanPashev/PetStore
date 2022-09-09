namespace PetStore.Data.Models.Common
{
    public static class CategoryValidationConstants
    {
        // Error Messages
        public const string NameIsRequired = "Product name is required!";
        public const string NameMinLengthMessage = "The name can not be less than 3 characters";
        public const string NameMaxLengthMessage = "The name can not be more than 50 characters long";
        public const string InvalidUrlMessage = "The image URL is not valid!";

        // Constrains
        public const int NameMinLength = 3;
        public const int NameMaxLength = 50;

        public const string UrlRegex = @"^(https://)(.)+";
        public const string UrlRegexForView = "^(https://)(.)+";

        public const string DefaultDateTimeFormat = "dd/MM/yyyy, HH:mm:ss";

    }
}
