namespace PetStore.Web.ViewModels.Address
{
    using System.ComponentModel.DataAnnotations;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Data.Models.Common;
    using PetStore.Services.Mapping;

    public class EditAddressViewModel : IMapFrom<Address>, IMapTo<Address>
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [RegularExpression(UserValidationConstants.DeliveryAddressRrgex, ErrorMessage = GlobalConstants.UserDeliveryAddressErrormessage)]
        public string AddressText { get; set; }

        [Required]
        [MaxLength(AddressValidationConstants.TownNameMaxLength)]
        public string TownName { get; set; }
    }
}
