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

        IQueryable<Product> GetDeletedProductsNoTracking();

        Task<Product> GetByIdAsync(string id);

        Task<Product> GetDeletedProductByIdAsyncNoTracking(string id);

        Task<Product> GetDeletedProductByIdAsync(string id);

        Task<Product> GetByIdForEditAsync(string id);

        bool IsProductEdited(ProductInfoViewModel model, Product product);

        bool IsProductExistingInDb(string productName);

        Task UpdateProductAsync(ProductInfoViewModel model, Product product);

        Task UndeleteAsync(Product product);
    }
}
