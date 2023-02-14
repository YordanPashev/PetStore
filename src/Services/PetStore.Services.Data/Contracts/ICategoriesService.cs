namespace PetStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.Categories;

    public interface ICategoriesService
    {
        Task AddCategoryAsync(Category category);

        IQueryable<Category> GetAllCategories();

        IQueryable<Category> GetAllCategoriesNoTracking();

        Task<Category> GetByIdAsync(int id);

        Task<Category> GetByIdNoTrackingAsync(int id);

        string GetCategoryImageUrl(string categoryName);

        Task<Category> GetCategoryWithDeletedProductsByIdAsync(string name);

        Task<int> GetCategoryIdByNameNoTrackingAsync(string name);

        bool IsCategoryExistingInDb(string name);

        bool IsCategoryEdited(Category category, CategoryProdutsViewModel userInputCategory);

        Task UpdateCategoryAsync(Category category, CategoryProdutsViewModel userInputCategory);
    }
}
