using System.Threading.Tasks;

namespace PetStore.Services.Data.Contracts
{
    public interface IAddressService
    {
        Task CreateNewAddress(string addressId, string userId, string fullAddressText);
    }
}
