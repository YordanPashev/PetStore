namespace PetStore.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;

    public class RequestsService : IRequestsService
    {
        private readonly IDeletableEntityRepository<Request> requestsRepo;

        public RequestsService(IDeletableEntityRepository<Request> requestsRepo)
            => this.requestsRepo = requestsRepo;

        public async Task CreateRequestAsync(Request userRequestModel)
        {
            userRequestModel.Id = Guid.NewGuid().ToString();
            await this.requestsRepo.AddAsync(userRequestModel);
            await this.requestsRepo.SaveChangesAsync();
        }

        public IQueryable<Request> GetAllActiveRequests()
            => this.requestsRepo.AllAsNoTracking()
                                .OrderByDescending(r => r.CreatedOn);

        public IQueryable<Request> GetAllInactiveRequests()
            => this.requestsRepo.AllAsNoTrackingWithDeleted()
                                .Where(r => r.IsDeleted)
                                .OrderByDescending(r => r.CreatedOn);

        public async Task<Request> GetRequestByIdASync(string id)
            => await this.requestsRepo.All()
                                      .Where(r => r.Id == id)
                                      .FirstOrDefaultAsync();

        public async Task RemoveRequestAsync(Request request)
        {
            this.requestsRepo.Delete(request);
            await this.requestsRepo.SaveChangesAsync();
        }
    }
}
