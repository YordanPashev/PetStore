namespace PetStore.Services.Data
{
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
            => this.categoriesRepo.AllAsNoTracking().Include(c => c.Products);

        public async Task<Category> GetById(int id)
            => await this.categoriesRepo
                    .AllAsNoTracking()
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.Id == id);
    }
}
