namespace PetStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.Products;

    public interface IProductsService
    {
        Task AddProductAsync(Product product);

        Task DeleteAsync(Product product);

        IQueryable<Product> GetAllProductsInSale();

        IQueryable<Product> GetAllProductsInSaleForSelectedCateogry(string categoryName);

        IQueryable<Product> GetDeletedProductsNoTracking();

        Task<Product> GetByIdAsync(string id);

        Task<Product> GetDeletedProductByIdAsyncNoTracking(string id);

        Task<Product> GetDeletedProductByIdAsync(string id);

        Task<Product> GetByIdForEditAsync(string id);

        bool IsProductExistingInDb(string productName);

        Task UpdateProductDataAsync(ProductInfoViewModel model, Product product);

        Task UndeleteAsync(Product product);
    }
}
