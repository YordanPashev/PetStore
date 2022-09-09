namespace PetStore.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Services.Mapping;
    using PetStore.Web.Controllers.Common;
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
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create(string errorMessage = null)
        {
           return this.View("Create", errorMessage);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Create(InputCategoryViewModel model)
        {
            if (!this.TryValidateModel(model, nameof(InputCategoryViewModel)))
            {
                return this.RedirectToAction("Create", "Categories", new { errorMessage = ValidationMessages.InvalidData });
            }

            if (this.categoriesService.IsCategoryExistingInDb(model.Name))
            {
                return this.RedirectToAction("Create", "Categories", new { errorMessage = ValidationMessages.CategoryAlreadyExistInDb });
            }

            Category category = AutoMapperConfig.MapperInstance.Map<Category>(model);
            await this.categoriesService.AddCategoryAsync(category);

            return this.RedirectToAction("SuccessfullyAddedCategory", "Categories", new RouteValueDictionary(model));
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
                AllCategories = allCategories.To<CategoryProdutsViewModel>().ToList(),
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

            CategoryProdutsViewModel categoryModel = AutoMapperConfig.MapperInstance.Map<CategoryProdutsViewModel>(category);
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

            CategoryProdutsViewModel deletedcategoryProductsModel = AutoMapperConfig.MapperInstance.Map<CategoryProdutsViewModel>(category);
            return this.View(deletedcategoryProductsModel);
        }

        [HttpGet]
        public IActionResult NoCategoryFound() => this.View();

        [HttpGet]
        public IActionResult SuccessfullyAddedCategory(InputCategoryViewModel model) => this.View(model);
    }
}
