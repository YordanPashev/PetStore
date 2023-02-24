namespace PetStore.Services.Data.Contracts
{
    using PetStore.Data.Models;
    using System.Threading.Tasks;

    public interface IClientCardService
    {
        Task CreateNewCard(string cardId, string userId);

        Task<ClientCard> GetCardByIdAsync(string clientCardId);
    }
}
