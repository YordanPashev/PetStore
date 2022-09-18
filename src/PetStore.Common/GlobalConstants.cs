namespace PetStore.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "PetStore";

        public const string AdministratorRoleName = "Administrator";

        public const string CategoryNotFoundErrorMessage = "Invalid Category.";

        public const string ProductAlreadyExistInDbErrorMessage = "Product already exist.";

        public const string CategoryAlreadyExistInDbErrorMessage = "Category exist.";

        public const string InvalidDataErrorMessage = "Invalid data.";

        public const string NothingWasEditedErrorMessage = "No changes were made.";

        public const string SuccessfullyDeleteProductMessage = $"The product has been removed from the Shop.";

        public const string SuccessfullyUndeleteProductMessage = $"The product is now available.";

        public const string SuccessfullyAddProductMessage = $"New product has been add:";

        public const string SuccessfullyEditProductMessage = $"The product has been edited:";
    }
}
