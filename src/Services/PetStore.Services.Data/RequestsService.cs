namespace PetStore.Services.Data
{
    using System.Threading.Tasks;

    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;

    public class RequestsService : IRequestsService
    {
        private readonly IDeletableEntityRepository<Request> requestsRepo;

        public RequestsService(IDeletableEntityRepository<Request> requestsRepo)
            => this.requestsRepo = requestsRepo;

        public async Task CreateRequest(Request userRequestModel)
        {
            await this.requestsRepo.AddAsync(userRequestModel);
            await this.requestsRepo.SaveChangesAsync();
        }
    }
}
