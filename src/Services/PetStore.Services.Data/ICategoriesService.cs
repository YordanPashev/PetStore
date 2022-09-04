namespace PetStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;

    public interface ICategoriesService
    {
        IQueryable<Category> GetAllCategories();

        Task<Category> GetAllDeletedCategoryProductsByIdAsync(int id);

        Task<Category> GetByIdAsync(int id);

        IQueryable<Category> GetAllCategoriesNoTracking();

        Task<Category> GetByIdNoTrackingAsync(int id);
    }
}
