namespace PetStore.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Categories;
    using PetStore.Web.ViewModels.Products;

    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        [HttpGet]
        public IActionResult Categories()
        {
            IQueryable allProducts = this.categoriesService.GetAllCategories();

            AllCategoriesViewModel categories = new AllCategoriesViewModel()
            {
                AllCategories = allProducts.To<CategoryViewModel>().ToArray(),
            };

            return this.View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryProducts(int id)
        {
            Category product = await this.categoriesService.GetById(id);
            if (product == null)
            {
                this.RedirectToAction("Error", "Home");
            }

            CategoryViewModel categoryProducts =
                AutoMapperConfig.MapperInstance.Map<CategoryViewModel>(product);

            return this.View(categoryProducts);
        }
    }
}
