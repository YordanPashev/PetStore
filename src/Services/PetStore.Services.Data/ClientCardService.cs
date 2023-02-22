namespace PetStore.Services.Data
{
    using System;

    using System.Threading.Tasks;
    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;

    public class ClientCardService : IClientCardService
    {
        private readonly IDeletableEntityRepository<ClientCard> clientCardRepo;

        public ClientCardService(IDeletableEntityRepository<ClientCard> productRepo)
            => this.clientCardRepo = productRepo;

        public async Task CreateNewCard(string cardId, string userId)
        {
            ClientCard clientCard = new ClientCard
            {
                Id = cardId,
                CardNumber = this.CenerateCardNumber(),
                ExpirationDate = this.GetExpirationDate(),
                Discount = this.GetDiscount(),
                ClientId = userId,
            };

            await this.clientCardRepo.AddAsync(clientCard);
            await this.clientCardRepo.SaveChangesAsync();
        }

        private int GetDiscount()
        {
            if (DateTime.Now.Month > 10)
            {
                return 20;
            }
            else if (DateTime.Now.Month < 5)
            {
                return 25;
            }

            return 10;
        }

        private DateTime GetExpirationDate()
            => DateTime.Now.AddYears(1);

        private string CenerateCardNumber()
        {
            int min = 100000;
            int max = 999999;
            Random randon = new Random();
            return randon.Next(min, max).ToString();
        }
    }
}
