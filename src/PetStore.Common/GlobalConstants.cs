namespace PetStore.Common
{
    using PetStore.Data.Models.Common;

    public static class GlobalConstants
    {
        // System
        public const string SystemName = "PetStore";

        public const string AdministratorRoleName = "Administrator";

        // User messages
        public const string SuccessfullyDeleteProductMessage = $"The selected product has been removed from the Shop.";

        public const string SuccessfullyUndeleteProductMessage = $"The selected product is now available in the shop.";

        public const string SuccessfullyUndeletePetMessage = $"The selected pet is now available in the shop.";

        public const string SuccessfullyDeletePetMessage = $"The selected pet has been removed from the Shop.";

        public const string SuccessfullyUserDeactivateHisAccountMessage = $"Your account has been activate. If you would like to reactivate your account, please send a request to the administrators.";

        public const string SuccessfullyAddedProductMessage = $"A new product has been add to the shop:";

        public const string SuccessfullyAddedProducCategoryMessage = $"A new category has been add to the shop";

        public const string SuccessfullyAddedPetMessage = $"A new pet has been add to the shop";

        public const string SuccessfullyEditedProductCategoryMessage = $"The category has been edited successfully";

        public const string SuccessfullyEditProductMessage = $"Edited result:";

        public const string ProductStatusInStock = "InStock";

        public const string ProductStatusDeleted = "Deleted";

        public const string PetlreadyExistInDbErrorMessage = "Pet has already been added to the shop.";

        public const string InvalidDataErrorMessage = "Invalid data.";

        public const string InvalidUrlUserMessage = "Include https://";

        public const string CategoryNotFoundErrorMessage = "Invalid Category.";

        public const string EditMessage = "Please make your changes.";

        public const string ProductAlreadyExistInDbErrorMessage = "Product with the same name already exist in the shop.";

        public const string ProductCategoryAlreadyExistInDbErrorMessage = "Category exist.";

        public const string UserNameMinLengthErrormessage = $"The name must be at least 3 characters";

        public const string UserNameMaxLengthErrormessage = "The name must be max 100 characters";

        public const string UserDeliveryAddressErrormessage = "The address must in format City, Street and max 150 characters!";

        public const string UserPhoneNumberErrormessage = "The phone number must contains only digits and must be min 7 and max 12!";

        public const string UserEmailErrorMessage = "The email must be in format  username@domainname.extension";

        // Account Statuses
        public const string AccountStatusActive = "Active";

        public const string AccountStatusInactive = "Inactive";

        // Date formats
        public const string AdministrationSettingsDateTimeFormat = "dd/MM/yyyy, HH:mm:ss";

        public const string PetBirthDateFormat = "d MMMM, yyyy";

        public const string DefaultDateDateFormat = "dd/MM/yyyy";

        // Pet Types
        public const string DogTypeImage = "https://media.istockphoto.com/id/1278389684/photo/large-group-of-various-breeds-of-dogs-together-on-a-white-background.jpg?s=612x612&w=0&k=20&c=MONWoLtCAUTJUbWed01JaLSgbBMclRbFCJ4szEK7iS0=";

        public const string CatTypeImage = "https://thumbs.dreamstime.com/b/four-cute-cats-20650677.jpg";

        public const string BirdTypeImage = "https://i.pinimg.com/originals/7a/a3/1b/7aa31be92644e466a338d52e2d7bc224.jpg";

        public const string FishTypeImage = "https://www.worldatlas.com/r/w1200/upload/04/ab/d1/fish-species-tropical.jpg";

        public const string RodentTypeImage = "https://www.earlham.ac.uk/sites/default/files/images/articles/Rodents-are-awesome/Rodents-are-awesome-extreme-evolution-feature-hero.jpg";

        public const string CreateProductImage = "https://patchpuppy.com/wp-content/uploads/2019/02/34369600_l.jpg.webp";

        public const string CreateCategoryImage = "https://www.linkpicture.com/q/Create-Category-Image.jpg";

        public const string AddPetImage = "https://media.istockphoto.com/id/1283692900/photo/group-of-different-kind-of-pets-like-cat-dog-rabbit-mouse-chinchilla-guinea-pig-bird-and-fish.jpg?s=612x612&w=0&k=20&c=oRyN_o5XT6UYFtwidjPxYFN9T5v0dBvu4CCAZUUJtW4=";
    }
}
