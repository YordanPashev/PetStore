﻿namespace PetStore.Data.Models.Common
{
    public static class UserValidationConstants
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 100;

        public const string PhoneNumberRrgex = @"^(\+)?(\d){7,12}$";

        public const string DeliveryAddressRrgex = @"(.){3,75},(.){5,75}";

        public const int DeliveryAddressMaxLength= 150;

        public const int PassWordMinLength = 6;
        public const int PassWordMaxLength = 100;
    }
}
