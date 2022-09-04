namespace PetStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.Products;

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productRepo;

        public ProductsService(IDeletableEntityRepository<Product> productRepo)
            => this.productRepo = productRepo;

        public async Task AddProductAsync(Product product)
        {
           await this.productRepo.AddAsync(product);
           await this.productRepo.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            this.productRepo.Delete(product);
            await this.productRepo.SaveChangesAsync();
        }

        public IQueryable<Product> GetAllProducts()
            => this.productRepo.AllAsNoTracking()
                    .Include(p => p.Category);

        public IQueryable<Product> GetDeletedProducts()
            => this.productRepo.AllAsNoTrackingWithDeleted()
                    .Include(p => p.Category)
                    .Where(p => p.IsDeleted == true);

        public async Task<Product> GetByIdAsync(string id)
            => await this.productRepo
                    .AllAsNoTracking()
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Product> GetByIdForEditAsync(string id)
            => await this.productRepo
                    .All()
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Product> GetDeletedProductsByIdAsync(string id)
            => await this.productRepo
                    .AllAsNoTrackingWithDeleted()
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

        public async Task UpdateProductAsync(Product product, ProductEditViewModel model)
        {
            product.Name = model.Name;
            product.Price = model.Price;
            product.Description = model.Description;
            product.ImageUrl = model.ImageUrl;
            product.CategoryId = model.CategoryId;

            await this.productRepo.SaveChangesAsync();
        }
    }
}
