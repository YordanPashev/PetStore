namespace PetStore.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Web.Infrastructures;

    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;
        private readonly CategoriesControllerExtension categoryControllerExtension;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
            this.categoryControllerExtension = new CategoriesControllerExtension(categoriesService);
        }

        [HttpGet]
        public IActionResult Index(string message = null)
        {
            IQueryable<Category> allCategories = this.categoriesService.GetAllCategoriesNoTracking();

            return this.categoryControllerExtension.ViewOrNoCategoryFound(allCategories, message);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryProducts(int id)
        {
            string productStatus = GlobalConstants.ProductStatusInStock;
            Category category = await this.categoriesService.GetByIdNoTrackingAsync(id);

            return this.categoryControllerExtension.RedirectOrNotFound(category, productStatus);
        }
    }
}
