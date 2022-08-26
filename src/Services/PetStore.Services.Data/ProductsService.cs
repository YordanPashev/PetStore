namespace PetStore.Services.Data
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productRepo;

        public ProductsService(IDeletableEntityRepository<Product> productRepo)
        {
            this.productRepo = productRepo;
        }

        public IQueryable<Product> GetAllProducts()
            => this.productRepo.AllAsNoTracking().Include(p => p.Category);
    }
}
