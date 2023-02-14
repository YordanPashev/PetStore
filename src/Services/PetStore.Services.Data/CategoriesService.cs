namespace PetStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.Categories;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepo;

        public CategoriesService(IDeletableEntityRepository<Category> productRepo)
            => this.categoriesRepo = productRepo;

        public async Task AddCategoryAsync(Category category)
        {
            await this.categoriesRepo.AddAsync(category);
            await this.categoriesRepo.SaveChangesAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
            => await this.categoriesRepo
                    .All()
                    .Include(c => c.Products.OrderBy(p => p.Name))
                    .FirstOrDefaultAsync(c => c.Id == id);

        public IQueryable<Category> GetAllCategories()
            => this.categoriesRepo
                     .All().Include(c => c.Products.OrderBy(p => p.Name)).
                     OrderBy(c => c.Name);

        public IQueryable<Category> GetAllCategoriesNoTracking()
            => this.categoriesRepo.AllAsNoTracking()
                     .Include(c => c.Products.OrderBy(p => p.Name))
                     .OrderBy(c => c.Name);

        public string GetCategoryImageUrl(string categoryName)
            => this.categoriesRepo.AllAsNoTracking()
                    .FirstOrDefault(c => c.Name == categoryName)
                    .ImageURL;

        public async Task<Category> GetCategoryWithDeletedProductsByIdAsync(string name)
            => await this.categoriesRepo.AllAsNoTrackingWithDeleted()
                    .Include(c => c.Products.Where(p => p.IsDeleted == true).OrderBy(p => p.Name))
                    .FirstOrDefaultAsync(c => c.Name == name);

        public async Task<Category> GetByIdNoTrackingAsync(int id)
            => await this.categoriesRepo
                    .AllAsNoTracking()
                    .Include(c => c.Products.OrderBy(p => p.Name))
                    .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<int> GetCategoryIdByNameNoTrackingAsync(string name)
        {
            Category category = await this.categoriesRepo.AllAsNoTracking()
                    .FirstOrDefaultAsync(c => c.Name == name);

            if (category == null)
            {
                return -1;
            }

            return category.Id;
        }

        public bool IsCategoryExistingInDb(string name)
            => this.categoriesRepo.AllAsNoTracking().Any(c => c.Name == name);

        public bool IsCategoryEdited(Category category, CategoryProdutsViewModel userInputCategory)
        {
            if (category.Id == userInputCategory.Id &&
                category.Name == userInputCategory.Name &&
                category.ImageURL == userInputCategory.ImageURL)
            {
                return false;
            }

            return true;
        }

        public async Task UpdateCategoryAsync(Category category, CategoryProdutsViewModel userInputCategory)
        {
            category.Name = userInputCategory.Name;
            category.ImageURL = userInputCategory.ImageURL;

            await this.categoriesRepo.SaveChangesAsync();
        }
    }
}
