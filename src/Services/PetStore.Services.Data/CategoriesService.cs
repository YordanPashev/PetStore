namespace PetStore.Services.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Services.Mapping;
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

        public async Task DeleteCategoryAsync(Category category)
        {
            this.categoriesRepo.Delete(category);
            await this.categoriesRepo.SaveChangesAsync();
        }

        public async Task<Category> GetDeletedCategoryByIdAsync(int id)
            => await this.categoriesRepo
                    .AllWithDeleted()
                    .Where(c => c.IsDeleted)
                    .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Category> GetByIdForEditAsync(int id)
            => await this.categoriesRepo
                    .All()
                    .Include(c => c.Products.OrderBy(p => p.Name))
                    .FirstOrDefaultAsync(c => c.Id == id);

        public ICollection<DeletedCategoryViewModel> GetDeletedCategories()
            => this.categoriesRepo
                     .AllAsNoTrackingWithDeleted()
                     .Where(c => c.IsDeleted)
                     .OrderBy(c => c.Name)
                     .To<DeletedCategoryViewModel>()
                     .ToList();

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

        public bool IsCategoryEdited(Category category, EditCategoryViewModel userInputCategory)
        {
            if (category.Id == userInputCategory.Id &&
                category.Name == userInputCategory.Name &&
                category.ImageURL == userInputCategory.ImageURL)
            {
                return false;
            }

            return true;
        }

        public async Task UpdateCategoryAsync(Category category, EditCategoryViewModel userInputCategory)
        {
            category.Name = userInputCategory.Name;
            category.ImageURL = userInputCategory.ImageURL;

            await this.categoriesRepo.SaveChangesAsync();
        }

        public async Task UndeleteCategoryAsync(Category category)
        {
            category.IsDeleted = false;
            category.DeletedOn = null;
            await this.categoriesRepo.SaveChangesAsync();
        }
    }
}
