namespace PetStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;

    public interface ICategoriesService
    {
        Task AddCategoryAsync(Category category);

        IQueryable<Category> GetAllCategories();

        Task<Category> GetByIdAsync(int id);

        IQueryable<Category> GetAllCategoriesNoTracking();

        Task<Category> GetAllDeletedCategoryProductsByIdAsync(int id);

        Task<Category> GetByIdNoTrackingAsync(int id);

        Task<int> GetIdByNameNoTrackingAsync(string name);

        bool IsCategoryExistingInDb(string name);
    }
}
