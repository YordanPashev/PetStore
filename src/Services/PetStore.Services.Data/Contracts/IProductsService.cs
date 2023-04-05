namespace PetStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.Products;

    public interface IProductsService
    {
        Task AddProductAsync(Product product);

        Task DeleteProductAsync(Product product);

        IQueryable<Product> GetAllProductsInSale(string orderCriteria);

        IQueryable<Product> GetAllCategoryProductsInSale(string categoryName, string orderCriteria);

        IQueryable<Product> GetAllDeletedProducts(string orderCriteria);

        IQueryable<Product> GetAllSearchedProductsOutOfStock(string searchQueryCapitalCase, string orderCriteria);

        IQueryable<Product> GetAllSearchedProductsInSale(string searchQueryCapitalCase, string orderCriteria);

        IQueryable<Product> GetAllSearchedCategoryProductsInSale(string searchQueryCapitalCase, string categoryName, string orderCriteria);

        Task<Product> GetByProductIdAsync(string id);

        Task<Product> GetDeletedProductByIdAsyncNoTracking(string id);

        Task<Product> GetDeletedProductByIdAsync(string id);

        Task<Product> GetProductByIdForEditAsync(string id);

        bool IsProductExistingInDb(string productName);

        Task UpdateProductDataAsync(ProductInfoViewModel model, Product product);

        Task UndeleteProductAsync(Product product);
    }
}
