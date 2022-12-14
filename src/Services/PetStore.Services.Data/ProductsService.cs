namespace PetStore.Services.Data
{
    using System;
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
           product.Price = Math.Round(product.Price, 2);
           await this.productRepo.AddAsync(product);
           await this.productRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            this.productRepo.Delete(product);
            await this.productRepo.SaveChangesAsync();
        }

        public IQueryable<Product> GetAllProductsInSale()
            => this.productRepo.AllAsNoTracking()
                    .Include(p => p.Category)
                    .OrderBy(p => p.Name);

        public IQueryable<Product> GetDeletedProductsNoTracking()
            => this.productRepo.AllAsNoTrackingWithDeleted()
                    .Include(p => p.Category)
                    .Where(p => p.IsDeleted)
                    .OrderBy(p => p.Name);

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

        public async Task<Product> GetDeletedProductByIdAsyncNoTracking(string id)
            => await this.productRepo
                    .AllAsNoTrackingWithDeleted()
                    .Where(p => p.IsDeleted)
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Product> GetDeletedProductByIdAsync(string id)
            => await this.productRepo
                    .AllWithDeleted()
                    .Where(p => p.IsDeleted)
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

        public bool IsProductEdited(ProductInfoViewModel model, Product product)
        {
            if (product.Name == model.Name && product.Price == model.Price && product.Description == model.Description &&
                product.ImageUrl == model.ImageUrl && product.CategoryId == model.CategoryId)
            {
                return false;
            }

            return true;
        }

        public bool IsProductExistingInDb(string productName)
            => this.productRepo
                   .AllAsNoTracking()
                   .Any(p => p.Name == productName);

        public async Task UpdateProductAsync(ProductInfoViewModel model, Product product)
        {
            product.Name = model.Name;
            product.Price = Math.Round(model.Price, 2);
            product.Description = model.Description;
            product.ImageUrl = model.ImageUrl;
            product.CategoryId = model.CategoryId;

            await this.productRepo.SaveChangesAsync();
        }

        public async Task UndeleteAsync(Product product)
        {
            product.DeletedOn = null;
            product.IsDeleted = false;
            await this.productRepo.SaveChangesAsync();
        }
    }
}
