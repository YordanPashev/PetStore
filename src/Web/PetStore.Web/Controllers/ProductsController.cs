namespace PetStore.Web.Controllers
{
    using System;
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

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;
        private readonly ICategoriesService categoriesService;
        private readonly ProductsControllerExtension productsControllerExtension;

        public ProductsController(IProductsService productService, ICategoriesService categoriesService)
        {
            this.productsService = productService;
            this.categoriesService = categoriesService;
            this.productsControllerExtension = new ProductsControllerExtension(productService);
        }

        [HttpGet]
        public IActionResult Index(string search)
        {
            ListOfProductsViewModel productsShortInfoModel = new ListOfProductsViewModel()
            {
                ListOfProducts = this.productsService.GetAllProductsInSale().To<ProductShortInfoViewModel>().ToArray(),
            };

            return this.productsControllerExtension.ViewOrNoProductsFound(productsShortInfoModel, search);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> DeleteConfirmation(string id)
        {
            Product product = await this.productsService.GetByIdAsync(id);
            return this.productsControllerExtension.ViewOrNoProductsFound(product);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> DeleteResult(string id)
        {
            string action = "Delete";
            Product product = await this.productsService.GetByIdAsync(id);

            return await this.productsControllerExtension.RedirectToSuccessfulOperationOrNoProductFound(product, action);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult DeletedProducts(string search)
        {
            ListOfProductsViewModel deletedProductsModel = new ListOfProductsViewModel()
            {
                ListOfProducts = this.productsService.GetDeletedProductsNoTracking().To<ProductShortInfoViewModel>().ToArray(),
            };

            return this.productsControllerExtension.ViewOrNoProductsFound(deletedProductsModel, search);
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
            Product product = await this.productsService.GetByIdAsync(id);

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
            Product product = await this.productsService.GetByIdForEditAsync(id);
            if (product == null)
            {
                return this.View("NoProductFound");
            }

            ProductWithAllCategoriesViewModel editPorudctModel = new ProductWithAllCategoriesViewModel()
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
            userInputModel.CategoryId = await this.categoriesService.GetIdByNameNoTrackingAsync(userInputModel.CategoryName);
            Product product = await this.productsService.GetByIdForEditAsync(userInputModel.Id);

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
            string action = "Undelete";
            Product product = await this.productsService.GetDeletedProductByIdAsync(id);

            return await this.productsControllerExtension.RedirectToSuccessfulOperationOrNoProductFound(product, action);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult SuccessfulOperationTextMessage(string message)
        {
            this.ViewBag.Message = message;
            return this.View();
        }
    }
}
