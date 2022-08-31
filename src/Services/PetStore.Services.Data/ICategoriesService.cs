namespace PetStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;

    public interface ICategoriesService
    {
        IQueryable<Category> GetAllCategories();

        Task<Category> GetById(int id);
    }
}
