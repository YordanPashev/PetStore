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

        public const string InvalidUrlUserMessage = "Include https://";

        public const string EditMessage = "Please make your changes.";

        public const string SuccessfullyDeleteProductMessage = $"The product has been removed from the Shop.";

        public const string SuccessfullyUndeleteProductMessage = $"The product is now available.";

        public const string SuccessfullyAddedProductMessage = $"A new product has been add to the shop:";

        public const string SuccessfullyAddedCategoryMessage = $"A new category has been add to the shop";

        public const string SuccessfullyEditedCategoryMessage = $"The category has been edited successfully";

        public const string SuccessfullyEditProductMessage = $"Edited result:";

        public const string ProductStatusInStock = "InStock";

        public const string ProductStatusDeleted = "Deleted";
    }
}
