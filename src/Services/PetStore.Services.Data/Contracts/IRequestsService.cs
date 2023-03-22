namespace PetStore.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PetStore.Data.Models;

    public interface IRequestsService
    {
        Task CreateRequest(Request userRequestModel);
    }
}
