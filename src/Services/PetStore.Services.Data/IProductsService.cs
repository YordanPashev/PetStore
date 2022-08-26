namespace PetStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;

    public interface IProductsService
    {
        IQueryable<Product> GetAllProducts();

        Task<Product> GetById(string id);
    }
}
