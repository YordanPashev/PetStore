namespace PetStore.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PetStore.Data.Models;

    public interface IClientCardService
    {
        Task CreateNewCard(string cardId, string userId);
    }
}
