namespace PetStore.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.Products;

    public interface IProductsService
    {
        Task AddProductAsync(Product product);

        Task DeleteProductAsync(Product product);

        IQueryable<Product> GetAllProductsInSale();

        IQueryable<Product> GetAllProductsInSaleForSelectedCateogry(string categoryName);

        IQueryable<Product> GetAllDeletedProductsNoTracking();

        ICollection<ProductShortInfoViewModel> GetAllSearchedProductsOutOfStockNoTracking(string searchQueryCapitalCase);

        ICollection<ProductShortInfoViewModel> GetAllSearchedProductsInSale(string searchQueryCapitalCase);

        ICollection<ProductShortInfoViewModel> GetAllSearchedProductsInSaleForSelectedCateogry(string searchQueryCapitalCase, string categoryName);

        Task<Product> GetByProductIdAsync(string id);

        Task<DetailsProductViewModel> GetDeletedProductByIdAsyncNoTracking(string id);

        Task<Product> GetDeletedProductByIdAsync(string id);

        Task<Product> GetProductByIdForEditAsync(string id);

        bool IsProductExistingInDb(string productName);

        Task UpdateProductDataAsync(ProductInfoViewModel model, Product product);

        Task UndeleteProductAsync(Product product);
    }
}
