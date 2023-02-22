namespace PetStore.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IClientCardService
    {
        Task CreateNewCard(string cardId, string userId);
    }
}
