namespace PetStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.Products;

    public interface IProductsService
    {
        IQueryable<Product> GetAllProducts();

        Task<Product> GetByIdAsync(string id);

        Task<Product> GetByIdForEditAsync(string id);

        Task AddProductAsync(Product product);

        Task UpdateProductAsync(Product product, ProductEditViewModel model, Category category);

        Task DeleteProductAsync(Product product);
    }
}
