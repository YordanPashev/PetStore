namespace PetStore.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "PetStore";

        public const string AdministratorRoleName = "Administrator";

        public const string CategoryNotFoundErrorMessage = "Invalid Category.";

        public const string ProductAlreadyExistInDbErrorMessage = "Product with the same name already exist in the shop.";

        public const string CategoryAlreadyExistInDbErrorMessage = "Category exist.";

        public const string InvalidDataErrorMessage = "Invalid data.";

        public const string InvalidUrlUserMessage = "Include https://";

        public const string EditMessage = "Please make your changes.";

        public const string SuccessfullyDeleteProductMessage = $"The product has been removed from the Shop.";

        public const string SuccessfullyUndeleteProductMessage = $"The product is now available.";

        public const string SuccessfullyAddedProductMessage = $"A new product has been add to the shop:";

        public const string SuccessfullyAddedCategoryMessage = $"A new category has been add to the shop";

        public const string SuccessfullyAddedPetMessage = $"A new pet has been add to the shop";

        public const string SuccessfullyEditedCategoryMessage = $"The category has been edited successfully";

        public const string SuccessfullyEditProductMessage = $"Edited result:";

        public const string ProductStatusInStock = "InStock";

        public const string ProductStatusDeleted = "Deleted";

        public const string PetlreadyExistInDbErrorMessage = "Pet has already been added to the shop.";

        public const string AdministrationSettingsDateTimeFormat = "dd/MM/yyyy, HH:mm:ss";

        public const string PetBirthDateFormat = "dd MMMM, yyyy";
    }
}
