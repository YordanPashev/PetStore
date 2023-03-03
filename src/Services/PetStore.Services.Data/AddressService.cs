namespace PetStore.Services.Data
{
    using System.Threading.Tasks;
    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;

    public class AddressService : IAddressService
    {
        private readonly IDeletableEntityRepository<Address> addressRepo;

        public AddressService(IDeletableEntityRepository<Address> addressRepo)
            => this.addressRepo = addressRepo;

        public async Task CreateNewAddress(string addressId, string userId, string fullAddressText)
        {
            int townseparatorIndex = fullAddressText.IndexOf(",");
            string townName = fullAddressText.Substring(0, townseparatorIndex).Trim();
            Address address = new Address()
            {
                Id = addressId,
                AddressText = fullAddressText,
                TownName = townName,
                ClientId = userId,
            };
            await this.addressRepo.AddAsync(address);
            await this.addressRepo.SaveChangesAsync();
        }
    }
}
