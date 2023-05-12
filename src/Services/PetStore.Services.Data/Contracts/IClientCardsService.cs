namespace PetStore.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IClientCardsService
    {
        Task CreateNewCard(string cardId, string userId);
    }
}
