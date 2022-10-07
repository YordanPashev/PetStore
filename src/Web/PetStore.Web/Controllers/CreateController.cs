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
    using PetStore.Web.ViewModels.Products;

    public class CreateController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly CreateControllerExtension createControllerExtension;

        public CreateController(IProductsService productService, ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
            this.createControllerExtension = new CreateControllerExtension(productService);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult CreateCategory(string errorMessage = null)
        {
            this.ViewBag.ErrorMessage = errorMessage;
            return this.View("CreateCategory");
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> CreateCategory(InputCategoryViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("CreateCategory", "Create", new { errorMessage = GlobalConstants.InvalidDataErrorMessage });
            }

            if (this.categoriesService.IsCategoryExistingInDb(model.Name))
            {
                return this.RedirectToAction("CreateCategory", "Create", new { errorMessage = GlobalConstants.CategoryAlreadyExistInDbErrorMessage });
            }

            Category category = AutoMapperConfig.MapperInstance.Map<Category>(model);
            await this.categoriesService.AddCategoryAsync(category);

            return this.RedirectToAction("Index", "Categories", new { message = GlobalConstants.SuccessfullyAddedCategoryMessage });
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult CreateProduct(string message = null)
        {
            ProductWithAllCategoriesViewModel createProductModel = new ProductWithAllCategoriesViewModel()
            {
                Categories = this.categoriesService.GetAllCategoriesNoTracking()
                                                   .To<CategoryShortInfoViewModel>()
                                                   .ToArray(),
                UserMessage = message,
            };

            return this.createControllerExtension.ViewOrNoGategoryFound(createProductModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> CreateProduct(ProductInfoViewModel userInputModel)
        {
            userInputModel.CategoryId = await this.categoriesService.GetIdByNameNoTrackingAsync(userInputModel.CategoryName);
            if (userInputModel == null || !this.ModelState.IsValid || userInputModel.CategoryId < 0)
            {
                return this.RedirectToAction("CreateProduct", new { message = GlobalConstants.InvalidDataErrorMessage });
            }

            return await this.createControllerExtension.CreateAndRedirectOrReturnInvalidInputMessage(userInputModel);
        }
    }
}
