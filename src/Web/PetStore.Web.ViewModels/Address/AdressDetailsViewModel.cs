namespace PetStore.Web.ViewModels.Address
{
    using PetStore.Data.Models;
    using PetStore.Services.Mapping;

    public class AdressDetailsViewModel : IMapFrom<Address>, IMapTo<Address>
    {
        public string Id { get; set; }

        public string AddressText { get; set; }
    }
}
