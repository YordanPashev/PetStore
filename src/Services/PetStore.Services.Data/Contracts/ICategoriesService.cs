namespace PetStore.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.Categories;

    public interface ICategoriesService
    {
        Task AddCategoryAsync(Category category);

        Task DeleteCategoryAsync(Category category);

        ICollection<DeletedCategoryViewModel> GetDeletedCategories();

        IQueryable<Category> GetAllCategoriesNoTracking();

        Task<Category> GetByIdForEditAsync(int id);

        Task<Category> GetDeletedCategoryByIdAsync(int id);

        Task<Category> GetByIdNoTrackingAsync(int id);

        string GetCategoryImageUrl(string categoryName);

        Task<Category> GetCategoryWithDeletedProductsByIdAsync(string name);

        Task<int> GetCategoryIdByNameNoTrackingAsync(string name);

        bool IsCategoryExistingInDb(string name);

        bool IsCategoryEdited(Category category, EditCategoryViewModel userInputCategory);

        Task UpdateCategoryAsync(Category category, EditCategoryViewModel userInputCategory);

        Task UndeleteCategoryAsync(Category category);
    }
}
