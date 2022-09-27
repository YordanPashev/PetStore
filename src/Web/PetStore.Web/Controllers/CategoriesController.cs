namespace PetStore.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Categories;

    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IQueryable<Category> allCategories = this.categoriesService.GetAllCategoriesNoTracking();

            if (allCategories == null)
            {
                return this.View("NoCategoryFound");
            }

            AllCategoriesViewModel categoriesModel = new AllCategoriesViewModel()
            {
                AllCategories = allCategories.To<CategoryProdutsViewModel>().ToList(),
            };

            return this.View(categoriesModel);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(int id, string message = null)
        {
            Category category = await this.categoriesService.GetByIdAsync(id);
            if (category == null)
            {
                return this.View("NoCategoryFound");
            }

            EditCategoryViewModel model = AutoMapperConfig.MapperInstance.Map<EditCategoryViewModel>(category);
            this.ViewBag.UserMessage = message;

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(CategoryProdutsViewModel model)
        {
            Category category = await this.categoriesService.GetByIdAsync(model.Id);

            if (!this.ModelState.IsValid || category == null)
            {
                return this.RedirectToAction("Edit", new { id = model.Id, message = GlobalConstants.InvalidDataErrorMessage });
            }

            return this.View();
            //if (!categoriesService.IsProducEdited(model))
            //{

            //}
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create(string errorMessage = null)
        {
            this.ViewBag.ErrorMessage = errorMessage;
            return this.View("Create");
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Create(InputCategoryViewModel model)
        {
            if (!this.TryValidateModel(model, nameof(InputCategoryViewModel)))
            {
                return this.RedirectToAction("Create", "Categories", new { errorMessage = GlobalConstants.InvalidDataErrorMessage });
            }

            if (this.categoriesService.IsCategoryExistingInDb(model.Name))
            {
                return this.RedirectToAction("Create", "Categories", new { errorMessage = GlobalConstants.CategoryAlreadyExistInDbErrorMessage });
            }

            Category category = AutoMapperConfig.MapperInstance.Map<Category>(model);
            await this.categoriesService.AddCategoryAsync(category);

            return this.RedirectToAction("SuccessfullyAddedCategory", "Categories", new RouteValueDictionary(model));
        }

        [HttpGet]
        public async Task<IActionResult> CategoryProducts(int id)
        {
            Category category = await this.categoriesService.GetByIdNoTrackingAsync(id);
            if (category == null)
            {
                return this.View("NoCategoryFound");
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
                return this.View("NoCategoryFound");
            }

            CategoryProdutsViewModel deletedcategoryProductsModel = AutoMapperConfig.MapperInstance.Map<CategoryProdutsViewModel>(category);
            return this.View(deletedcategoryProductsModel);
        }

        [HttpGet]
        public IActionResult SuccessfullyAddedCategory(InputCategoryViewModel model) => this.View(model);
    }
}
