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
    using PetStore.Web.ViewModels.Products;
    using PetStore.Web.ViewModels.Search;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;
        private readonly ICategoriesService categoriesService;
        private readonly ProductsControllerExtension productsControllerExtension;

        public ProductsController(IProductsService productService, ICategoriesService categoriesService)
        {
            this.productsService = productService;
            this.categoriesService = categoriesService;
            this.productsControllerExtension = new ProductsControllerExtension(productService, categoriesService);
        }

        [HttpGet]
        public IActionResult Index(SearchProductViewModel model)
        {
            return this.productsControllerExtension.ViewOrNoProductsFound(model);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> DeleteConfirmation(string id)
        {
            Product product = await this.productsService.GetByProductIdAsync(id);
            return this.productsControllerExtension.ViewOrNoProductsFound(product);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> DeleteResult(string id)
        {
            Product product = await this.productsService.GetByProductIdAsync(id);
            if (product != null)
            {
                await this.productsService.DeleteProductAsync(product);
                this.ViewBag.Message = GlobalConstants.SuccessfullyDeleteProductMessage;

                return this.View("SuccessfulOperationTextMessage");
            }

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult DeletedProducts(SearchProductViewModel searchModel)
        {
            return this.productsControllerExtension.DeletedProductsViewOrNoProductsFound(searchModel.SearchQuery);
        }

        [HttpGet]
        public async Task<IActionResult> DeletedProductDetails(string id)
        {
            Product product = await this.productsService.GetDeletedProductByIdAsyncNoTracking(id);
            return this.productsControllerExtension.ViewOrNoProductsFound(product);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, string message = null)
        {
            Product product = await this.productsService.GetByProductIdAsync(id);

            if (product == null)
            {
                return this.View("NoProductFound");
            }

            DetailsProductViewModel productDetailsModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);
            productDetailsModel.UserMessage = message;
            return this.View(productDetailsModel);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(string id, string message = null)
        {
            Product product = await this.productsService.GetProductByIdForEditAsync(id);
            if (product == null)
            {
                return this.View("NoProductFound");
            }

            EditProductViewModel editPorudctModel = new EditProductViewModel()
            {
                ProductInfo = AutoMapperConfig.MapperInstance.Map<ProductInfoViewModel>(product),
                Categories = this.categoriesService.GetAllCategoriesNoTracking().To<CategoryShortInfoViewModel>().ToArray(),
                UserMessage = message,
            };

            return this.View(editPorudctModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(ProductInfoViewModel userInputModel)
        {
            userInputModel.CategoryId = await this.categoriesService.GetCategoryIdByNameNoTrackingAsync(userInputModel.CategoryName);
            Product product = await this.productsService.GetProductByIdForEditAsync(userInputModel.Id);

            if (!this.ModelState.IsValid || product == null || userInputModel?.CategoryId < 0)
            {
                return this.RedirectToAction("Edit", new { id = userInputModel.Id, message = GlobalConstants.InvalidDataErrorMessage });
            }

            return await this.productsControllerExtension.EditAndRedirectOrReturnInvalidInputMessage(userInputModel, product);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> UndeleteConfirmation(string id)
        {
            Product product = await this.productsService.GetDeletedProductByIdAsyncNoTracking(id);
            return this.productsControllerExtension.ViewOrNoProductsFound(product);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> UndeleteResult(string id)
        {
            Product product = await this.productsService.GetDeletedProductByIdAsync(id);
            if (product != null)
            {
                await this.productsService.UndeleteProductAsync(product);
                this.ViewBag.Message = GlobalConstants.SuccessfullyUndeleteProductMessage;

                return this.View("SuccessfulOperationTextMessage");
            }

            return this.RedirectToAction("Index");
        }
    }
}
