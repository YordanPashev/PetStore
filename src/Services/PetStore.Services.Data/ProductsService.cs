namespace PetStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productRepo;

        public ProductsService(IDeletableEntityRepository<Product> productRepo)
            => this.productRepo = productRepo;

        public IQueryable<Product> GetAllProducts()
            => this.productRepo.AllAsNoTracking().Include(p => p.Category);

        public async Task<Product> GetById(string id)
            => await this.productRepo.AllAsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    }
}
