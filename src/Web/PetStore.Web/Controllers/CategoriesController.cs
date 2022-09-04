namespace PetStore.Web.Controllers
{
    using System.Collections.Generic;
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
            IQueryable<Category> allCategories = this.categoriesService.GetAllCategoriesNoTracking();

            if (allCategories == null)
            {
                return this.RedirectToAction("NoCategoryFound", "Categories");
            }

            AllCategoriesViewModel categoriesModel = new AllCategoriesViewModel()
            {
                AllCategories = allCategories.To<CategoryViewModel>().ToList(),
            };

            return this.View(categoriesModel);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryProducts(int id)
        {
            Category category = await this.categoriesService.GetByIdNoTrackingAsync(id);
            if (category == null)
            {
                return this.RedirectToAction("NoCategoryFound", "Categories");
            }

            CategoryViewModel categoryModel = AutoMapperConfig.MapperInstance.Map<CategoryViewModel>(category);
            return this.View(categoryModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeletedCategoryPorducts(int id)
        {
            Category category = await this.categoriesService.GetAllDeletedCategoryProductsByIdAsync(id);
            if (category == null)
            {
                return this.RedirectToAction("NoCategoryFound", "Categories");
            }

            CategoryViewModel deletedcategoryProductsModel = AutoMapperConfig.MapperInstance.Map<CategoryViewModel>(category);
            return this.View(deletedcategoryProductsModel);
        }

        [HttpGet]
        public IActionResult NoCategoryFound()
            => this.View();
    }
}
