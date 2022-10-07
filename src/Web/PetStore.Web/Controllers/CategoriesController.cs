namespace PetStore.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Services.Mapping;
    using PetStore.Web.Infrastructures;
    using PetStore.Web.ViewModels.Categories;

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
        public async Task<IActionResult> Edit(CategoryProdutsViewModel userInputModel)
        {
            Category category = await this.categoriesService.GetByIdAsync(userInputModel.Id);

            if (category == null)
            {
                return this.View("NoCategoryFound");
            }

            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Edit", new { id = userInputModel.Id, message = GlobalConstants.InvalidDataErrorMessage });
            }

            return await this.categoryControllerExtension.EditAndRedirectOrReturnMessage(category, userInputModel);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryProducts(int id)
        {
            string productStatus = GlobalConstants.ProductStatusInStock;
            Category category = await this.categoriesService.GetByIdNoTrackingAsync(id);

            return this.categoryControllerExtension.RedirectOrNotFound(category, productStatus);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> DeletedCategoryPorducts(string name)
        {
            string productStatus = GlobalConstants.ProductStatusDeleted;
            Category category = await this.categoriesService.GetCategoryWithDeletedProductsByIdAsync(name);

            return this.categoryControllerExtension.RedirectOrNotFound(category, productStatus);
        }
    }
}
