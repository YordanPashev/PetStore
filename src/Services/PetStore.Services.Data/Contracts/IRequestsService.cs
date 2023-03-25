namespace PetStore.Services.Data.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;

    public interface IRequestsService
    {
        Task CreateRequestAsync(Request userRequestModel);

        IQueryable<Request> GetAllActiveRequests();

        IQueryable<Request> GetAllInactiveRequests();

        Task<Request> GetRequestByIdASync(string id);

        Task RemoveRequestAsync(Request request);
    }
}
