namespace PetStore.Services.Data
{
    using System.Linq;

    using PetStore.Data.Models;

    public interface IProductsService
    {
        IQueryable<Product> GetAllProducts();
    }
}
