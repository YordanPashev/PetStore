namespace PetStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.Products;

    public interface IProductsService
    {
        IQueryable<Product> GetAllProducts();

        IQueryable<Product> GetDeletedProducts();

        Task<Product> GetByIdAsync(string id);

        Task<Product> GetDeletedProductsByIdAsync(string id);

        Task<Product> GetByIdForEditAsync(string id);

        Task AddProductAsync(Product product);

        Task UpdateProductAsync(Product product, ProductEditViewModel model);

        Task DeleteProductAsync(Product product);
    }
}
