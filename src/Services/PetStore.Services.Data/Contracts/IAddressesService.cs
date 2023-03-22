namespace PetStore.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IAddressesService
    {
        Task CreateNewAddress(string addressId, string userId, string fullAddressText);
    }
}
