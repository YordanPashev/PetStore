namespace PetStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.Products;

    public interface IProductsService
    {
        Task AddProductAsync(Product product);

        IQueryable<Product> GetAllProducts();

        IQueryable<Product> GetDeletedProducts();

        Task<Product> GetByIdAsync(string id);

        Task<Product> GetDeletedProductsByIdAsync(string id);

        Task<Product> GetByIdForEditAsync(string id);

        Task UpdateProductAsync(Product product, ProductViewModel model);

        Task DeleteProductAsync(Product product);
    }
}
