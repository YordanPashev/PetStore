namespace PetStore.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepo;

        public CategoriesService(IDeletableEntityRepository<Category> productRepo)
            => this.categoriesRepo = productRepo;

        public IQueryable<Category> GetAllCategories()
            => this.categoriesRepo
                     .All().Include(c => c.Products.OrderBy(p => p.Name)).
                     OrderBy(c => c.Name);

        public IQueryable<Category> GetAllCategoriesNoTracking()
            => this.categoriesRepo.AllAsNoTracking()
                     .Include(c => c.Products.OrderBy(p => p.Name))
                     .OrderBy(c => c.Name);

        public async Task<Category> GetByIdAsync(int id)
            => await this.categoriesRepo
                    .All()
                    .Include(c => c.Products.OrderBy(p => p.Name))
                    .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Category> GetByIdNoTrackingAsync(int id)
            => await this.categoriesRepo
                    .AllAsNoTracking()
                    .Include(c => c.Products.OrderBy(p => p.Name))
                    .FirstOrDefaultAsync(c => c.Id == id);
    }
}
