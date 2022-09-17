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
    using PetStore.Web.Infrastructures;
    using PetStore.Web.ViewModels.Products;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;
        private readonly ICategoriesService categoriesService;
        private readonly ProductsControllerExtension controllerExtension;

        public ProductsController(IProductsService productService, ICategoriesService categoriesService)
        {
            this.productsService = productService;
            this.categoriesService = categoriesService;
            this.controllerExtension = new ProductsControllerExtension(productService);
        }

        [HttpGet]
        public IActionResult Index()
        {
            AllProductsViewModel productsShortInfoModel = new AllProductsViewModel()
            {
                ListOfProducts = this.productsService.GetAllProducts().To<DetailsProductViewModel>().ToArray(),
            };

            return this.controllerExtension.ViewOrNoProductsFound(productsShortInfoModel);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create(ProductInfoViewModel userInputModel = null)
        {
            ProductWithAllCategoriesViewModel createProductModel = new ProductWithAllCategoriesViewModel()
            {
                ProductInfo = userInputModel,
                Categories = this.categoriesService.GetAllCategoriesNoTracking()
                                                   .To<CategoryShortInfoViewModel>()
                                                   .ToArray(),
            };

            return this.controllerExtension.ViewOrNoGategoryFound(createProductModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> TryToCreate(ProductInfoViewModel userInputModel)
        {
            string action = "Create";
            userInputModel.CategoryId = await this.categoriesService.GetIdByNameNoTrackingAsync(userInputModel.CategoryName);

            if (this.productsService.IsProductExistingInDb(userInputModel.Name))
            {
                userInputModel.ErrorMessage = GlobalConstants.ProductAlreadyExistInDbErrorMessage;
                return this.RedirectToAction(action, "Products", userInputModel);
            }

            return await this.controllerExtension.SuccessfulOperationOrInvalidData(userInputModel, action);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(string id)
        {
            Product product = await this.productsService.GetByIdAsync(id);
            DetailsProductViewModel deletedProductModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);

            return this.controllerExtension.ViewOrNoProductsFound(deletedProductModel);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult DeletedProducts()
        {
            AllProductsViewModel deletedProductsModel = new AllProductsViewModel()
            {
                ListOfProducts = this.productsService.GetDeletedProductsNoTracking().To<DetailsProductViewModel>().ToArray(),
            };

            return this.controllerExtension.ViewOrNoProductsFound(deletedProductsModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeletedProductDetails(string id)
        {
            Product product = await this.productsService.GetDeletedProductByIdAsyncNoTracking(id);
            DetailsProductViewModel deletedProductModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);

            return this.controllerExtension.ViewOrNoProductsFound(deletedProductModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Product product = await this.productsService.GetByIdAsync(id);
            DetailsProductViewModel productDetailsModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);

            return this.controllerExtension.ViewOrNoProductsFound(productDetailsModel);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(string id, string errorMessage)
        {
            Product product = await this.productsService.GetByIdForEditAsync(id);

            ProductWithAllCategoriesViewModel editPorudctModel = new ProductWithAllCategoriesViewModel()
            {
                ProductInfo = AutoMapperConfig.MapperInstance.Map<ProductInfoViewModel>(product),
                Categories = this.categoriesService.GetAllCategoriesNoTracking().To<CategoryShortInfoViewModel>().ToArray(),
            };
            editPorudctModel.ProductInfo.ErrorMessage = errorMessage;
            return this.View(editPorudctModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(ProductInfoViewModel userInputModel)
        {
            string action = "Edit";
            userInputModel.CategoryId = await this.categoriesService.GetIdByNameNoTrackingAsync(userInputModel.CategoryName);
            Product product = await this.productsService.GetByIdForEditAsync(userInputModel.Id);

            if (!this.productsService.IsProductEdited(userInputModel, product))
            {
                return this.RedirectToAction("Edit", "Products", new { modelId = userInputModel.Id, errorMessage = ValidationMessages.NothingWasEdited });
            }

            return await this.controllerExtension.SuccessfulOperationOrInvalidData(userInputModel, action, product);
        }

        [HttpGet]
        public IActionResult NoProductFound() => this.View();

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult SuccessfullyAddedProduct(ProductInfoViewModel model) => this.View(model);

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> SuccessfullyDeletedProduct(string id)
        {
            string action = "delete";
            Product product = await this.productsService.GetByIdAsync(id);

            return await this.controllerExtension.ViewOrRedirectToAllProducts(product, action);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> SuccessfullyUndeletedProduct(string id)
        {
            string action = "undelete";
            Product product = await this.productsService.GetDeletedProductByIdAsync(id);

            return await this.controllerExtension.ViewOrRedirectToAllProducts(product, action);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult SuccessfullyEditedProduct(ProductInfoViewModel model) => this.View(model);

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Undelete(string id)
        {
            Product product = await this.productsService.GetDeletedProductByIdAsyncNoTracking(id);
            DetailsProductViewModel productDetailsModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);

            return this.controllerExtension.ViewOrNoProductsFound(productDetailsModel);
        }
    }
}
