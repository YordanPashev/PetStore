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

        public async Task DeleteProductAsync(Product product)
        {
            this.productRepo.Delete(product);
            await this.productRepo.SaveChangesAsync();
        }

        public IQueryable<Product> GetAllProductsInSale()
            => this.productRepo.AllAsNoTracking()
                    .Where(p => p.IsDeleted == false)
                    .Include(p => p.Category)
                    .OrderBy(p => p.Name);

        public IQueryable<Product> GetAllProductsInSaleForSelectedCateogry(string categoryName)
        {
            if (categoryName == null)
            {
                return Enumerable.Empty<Product>().AsQueryable();
            }

            return this.productRepo.AllAsNoTracking()
                    .Include(p => p.Category)
                    .Where(p => p.IsDeleted == false)
                    .Where(p => p.Category.Name == categoryName)
                    .OrderBy(p => p.Name);
        }

        public IQueryable<Product> GetAllDeletedProductsNoTracking()
            => this.productRepo.AllAsNoTrackingWithDeleted()
                    .Include(p => p.Category)
                    .Where(p => p.IsDeleted)
                    .OrderBy(p => p.Name);

        public async Task<Product> GetByProductIdAsync(string id)
            => await this.productRepo
                    .AllAsNoTracking()
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Product> GetProductByIdForEditAsync(string id)
            => await this.productRepo
                    .All()
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Product> GetDeletedProductByIdAsync(string id)
            => await this.productRepo
                    .AllWithDeleted()
                    .Where(p => p.IsDeleted)
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Product> GetDeletedProductByIdAsyncNoTracking(string id)
            => await this.productRepo
                    .AllAsNoTrackingWithDeleted()
                    .Where(p => p.IsDeleted)
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

        public bool IsProductExistingInDb(string productName)
            => this.productRepo
                   .AllAsNoTracking()
                   .Any(p => p.Name == productName);

        public async Task UpdateProductDataAsync(ProductInfoViewModel userInputModel, Product product)
        {
            product.Name = userInputModel.Name;
            product.Price = Math.Round(userInputModel.Price, 2);
            product.Description = userInputModel.Description;
            product.ImageUrl = userInputModel.ImageUrl;
            product.CategoryId = userInputModel.CategoryId;

            await this.productRepo.SaveChangesAsync();
        }

        public async Task UndeleteProductAsync(Product product)
        {
            product.DeletedOn = null;
            product.IsDeleted = false;
            await this.productRepo.SaveChangesAsync();
        }
    }
}
