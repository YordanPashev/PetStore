namespace PetStore.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PetStore.Common;
    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Services.Mapping;
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

        public IQueryable<Product> GetAllProductsInSale(string orderCriteria)
        {
            IQueryable<Product> listOfProducts = this.productRepo.AllAsNoTracking()
                                                                 .Where(p => p.IsDeleted == false)
                                                                 .Include(p => p.Category);
            listOfProducts = this.OrderByCriteria(orderCriteria, listOfProducts);

            return listOfProducts;
        }

        public IQueryable<Product> GetAllCategoryProductsInSale(string categoryName, string orderCriteria)
        {
            if (categoryName == null)
            {
                return Enumerable.Empty<Product>().AsQueryable();
            }

            IQueryable<Product> listOfProducts = this.productRepo.AllAsNoTracking()
                                                                 .Include(p => p.Category)
                                                                 .Where(p => p.Category.Name == categoryName);
            listOfProducts = this.OrderByCriteria(orderCriteria, listOfProducts);

            return listOfProducts;
        }

        public IQueryable<Product> GetAllDeletedProducts(string orderCriteria)
        {
            IQueryable<Product> listOfProducts = this.productRepo.AllAsNoTrackingWithDeleted()
                                                                 .Include(p => p.Category)
                                                                 .Where(p => p.IsDeleted);
            listOfProducts = this.OrderByCriteria(orderCriteria, listOfProducts);

            return listOfProducts;
        }

        public IQueryable<Product> GetAllSearchedProductsOutOfStock(string searchQueryCapitalCase, string orderCriteria)
        {
            IQueryable<Product> listOfProducts = this.productRepo.AllAsNoTrackingWithDeleted()
                                                                 .Where(p => p.IsDeleted)
                                                                 .Include(p => p.Category)
                                                                 .Where(p => p.Name.ToUpper().Contains(searchQueryCapitalCase) ||
                                                                             p.Category.Name.ToUpper().Contains(searchQueryCapitalCase));
            listOfProducts = this.OrderByCriteria(orderCriteria, listOfProducts);

            return listOfProducts;
        }

        public IQueryable<Product> GetAllSearchedProductsInSale(string searchQueryCapitalCase, string orderCriteria)
        {
            IQueryable<Product> listOfProducts = this.productRepo.AllAsNoTracking()
                                                                 .Include(p => p.Category)
                                                                 .Where(p => p.Name.ToUpper().Contains(searchQueryCapitalCase) ||
                                                                             p.Category.Name.ToUpper().Contains(searchQueryCapitalCase));
            listOfProducts = this.OrderByCriteria(orderCriteria, listOfProducts);

            return listOfProducts;
        }

        public IQueryable<Product> GetAllSearchedCategoryProductsInSale(string searchQueryCapitalCase, string categoryName, string orderCriteria)
        {
            if (categoryName == null)
            {
                return Enumerable.Empty<Product>().AsQueryable();
            }

            IQueryable<Product> listOfProducts = this.productRepo.AllAsNoTracking()
                                                                 .Include(p => p.Category)
                                                                 .Where(p => p.Category.Name == categoryName)
                                                                 .Where(p => p.Name.ToUpper().Contains(searchQueryCapitalCase) ||
                                                                             p.Category.Name.ToUpper().Contains(searchQueryCapitalCase));

            listOfProducts = this.OrderByCriteria(orderCriteria, listOfProducts);

            return listOfProducts;
        }

        public async Task<Product> GetProductByIdAsync(string id)
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

        private IQueryable<Product> OrderByCriteria(string orderCriteria, IQueryable<Product> listOfPets)
        {
            if (orderCriteria == GlobalConstants.CriteriaPriceAscending)
            {
                return listOfPets.OrderBy(p => p.Price)
                                 .ThenBy(p => p.Name);
            }
            else if (orderCriteria == GlobalConstants.CriteriaPriceDescending)
            {
                return listOfPets.OrderByDescending(p => p.Price)
                                 .ThenBy(p => p.Name);
            }
            else if (orderCriteria == GlobalConstants.CriteriaType)
            {
                return listOfPets.OrderBy(p => p.Category.Name)
                                 .ThenBy(p => p.Name);
            }
            else if (orderCriteria == GlobalConstants.CriteriaRecent)
            {
                return listOfPets.OrderByDescending(p => p.CreatedOn)
                                 .ThenBy(p => p.Name);
            }

            return listOfPets.OrderBy(p => p.Name);
        }
    }
}
