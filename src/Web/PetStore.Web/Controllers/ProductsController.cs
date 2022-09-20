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
                ListOfProducts = this.productsService.GetAllProducts().To<ProductShortInfoViewModel>().ToArray(),
            };

            return this.controllerExtension.ViewOrNoProductsFound(productsShortInfoModel);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create(string message = null)
        {
            ProductWithAllCategoriesViewModel createProductModel = new ProductWithAllCategoriesViewModel()
            {
                Categories = this.categoriesService.GetAllCategoriesNoTracking()
                                                   .To<CategoryShortInfoViewModel>()
                                                   .ToArray(),
                UserMessage = message,
            };

            return this.controllerExtension.ViewOrNoGategoryFound(createProductModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Create(ProductInfoViewModel userInputModel)
        {
            string action = "Create";
            userInputModel.CategoryId = await this.categoriesService.GetIdByNameNoTrackingAsync(userInputModel.CategoryName);

            return await this.controllerExtension.ProcessAndRedirectOrInvalidData(userInputModel, action);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> DeleteConfirmation(string id)
        {
            Product product = await this.productsService.GetByIdAsync(id);
            DetailsProductViewModel deletedProductModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);

            return this.controllerExtension.ViewOrNoProductsFound(deletedProductModel);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> DeleteResult(string id)
        {
            string action = "Delete";
            Product product = await this.productsService.GetByIdAsync(id);

            return await this.controllerExtension.ViewOrNoProductFound(product, action);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult DeletedProducts()
        {
            AllProductsViewModel deletedProductsModel = new AllProductsViewModel()
            {
                ListOfProducts = this.productsService.GetDeletedProductsNoTracking().To<ProductShortInfoViewModel>().ToArray(),
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
        public async Task<IActionResult> Details(string id, string message = null)
        {
            Product product = await this.productsService.GetByIdAsync(id);
            DetailsProductViewModel productDetailsModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);

            if (message != null && product != null)
            {
                productDetailsModel.UserMessage = message;
            }

            return this.controllerExtension.ViewOrNoProductsFound(productDetailsModel);
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
            string action = "Edit";
            userInputModel.CategoryId = await this.categoriesService.GetIdByNameNoTrackingAsync(userInputModel.CategoryName);
            Product product = await this.productsService.GetByIdForEditAsync(userInputModel.Id);

            return await this.controllerExtension.ProcessAndRedirectOrInvalidData(userInputModel, action, product);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> UndeleteConfirmation(string id)
        {
            Product product = await this.productsService.GetDeletedProductByIdAsyncNoTracking(id);
            DetailsProductViewModel productDetailsModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);

            return this.controllerExtension.ViewOrNoProductsFound(productDetailsModel);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> UndeleteResult(string id)
        {
            string action = "Undelete";
            Product product = await this.productsService.GetDeletedProductByIdAsync(id);

            return await this.controllerExtension.ViewOrNoProductFound(product, action);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult SuccessfullOperationTextMessage(string message)
        {
            this.ViewBag.Message = message;
            return this.View();
        }
    }
}
