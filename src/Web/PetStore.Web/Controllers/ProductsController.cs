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
            this.controllerExtension = new ProductsControllerExtension(productService, categoriesService);
        }

        [HttpGet]
        public IActionResult Index()
        {
            IQueryable<Product> allProducts = this.productsService.GetAllProducts();
            return this.controllerExtension.ViewOrNoProductsFound(allProducts);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create(CreateProductViewModel createProductModel = null)
        {
            ICollection<CategoryShortInfoViewModel> allCategoriesInfo = this.categoriesService.GetAllCategoriesNoTracking()
                                                                                              .To<CategoryShortInfoViewModel>()
                                                                                              .ToArray();

            return this.controllerExtension.ViewOrNoGategoryFound(createProductModel, allCategoriesInfo);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> TryToCreate(CreateProductViewModel userInputModel)
        {
            return await this.controllerExtension.SuccessfulyAddedProdcutOrInvalidData(userInputModel);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(string id)
        {
            Product product = await this.productsService.GetByIdAsync(id);
            if (product == null)
            {
                return this.RedirectToAction("NoProductFound", "Products");
            }

            DetailsProductViewModel deletedProductModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);
            return this.View(deletedProductModel);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult DeletedProducts()
        {
            IQueryable<Product> allProducts = this.productsService.GetDeletedProductsNoTracking();
            if (allProducts == null)
            {
                return this.RedirectToAction("NoProductFound", "Products");
            }

            AllProductsViewModel deletedProductsModel = new AllProductsViewModel()
            {
                AllProducts = allProducts.To<DetailsProductViewModel>().ToArray(),
            };

            return this.View(deletedProductsModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeletedProductDetails(string id)
        {
            Product product = await this.productsService.GetDeletedProductsByIdAsyncNoTracking(id);
            if (product == null)
            {
                return this.RedirectToAction("NoProductFound", "Products");
            }

            DetailsProductViewModel deletedProductModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);
            return this.View(deletedProductModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Product product = await this.productsService.GetByIdAsync(id);
            if (product == null)
            {
                return this.RedirectToAction("NoProductFound", "Products");
            }

            DetailsProductViewModel productDetailsModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);
            return this.View(productDetailsModel);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(string id, string errorMessage = null)
        {
            ICollection<CategoryShortInfoViewModel> allCategories =
                this.categoriesService.GetAllCategoriesNoTracking().To<CategoryShortInfoViewModel>().ToArray();
            Product product = await this.productsService.GetByIdForEditAsync(id);
            if (product == null)
            {
                return this.RedirectToAction("NoProductFound", "Products");
            }

            if (allCategories == null)
            {
                return this.RedirectToAction("NoCategoryFound", "Products");
            }

            EditProductInfoViewModel productModel = AutoMapperConfig.MapperInstance.Map<EditProductInfoViewModel>(product);

            EditProductFullInfoViewModel edinPorudctModel = new EditProductFullInfoViewModel()
            {
                Product = productModel,
                Categories = allCategories,
                ErrorMessage = errorMessage,
            };

            return this.View(edinPorudctModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(EditProductInfoViewModel model)
        {
            Category category = await this.categoriesService.GetByIdAsync(model.CategoryId);
            Product product = await this.productsService.GetByIdForEditAsync(model.Id);
            if (!this.TryValidateModel(model, nameof(EditProductInfoViewModel)) ||
                category == null || product == null)
            {
                return this.RedirectToAction("Edit", "Products", new { modelId = model.Id, errorMessage = ValidationMessages.InvalidData });
            }

            if (!this.productsService.IsProductEdited(model, product))
            {
                return this.RedirectToAction("Edit", "Products", new { modelId = model.Id, errorMessage = ValidationMessages.NothingWasEdited });
            }

            model.CategoryName = category.Name;
            await this.productsService.UpdateProductAsync(product, model);
            return this.RedirectToAction("SuccessfullyEditedProduct", "Products", new RouteValueDictionary(model));
        }

        [HttpGet]
        public IActionResult NoProductFound() => this.View();

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult SuccessfullyAddedProduct(CreateProductViewModel model) => this.View(model);

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> SuccessfullyDeletedProduct(string id)
        {
            Product product = await this.productsService.GetByIdAsync(id);
            if (product == null)
            {
                return this.RedirectToAction("AllProducts", "Products");
            }

            DetailsProductViewModel productDetailsModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);
            await this.productsService.DeleteProductAsync(product);

            return this.View(productDetailsModel);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> SuccessfullyUndeletedProduct(string id)
        {
            Product product = await this.productsService.GetDeletedProductsByIdAsync(id);
            if (product == null)
            {
                return this.RedirectToAction("AllProducts", "Products");
            }

            DetailsProductViewModel productDetailsModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);
            await this.productsService.UndeleteAsync(product);

            return this.View(productDetailsModel);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult SuccessfullyEditedProduct(EditProductInfoViewModel model) => this.View(model);

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Undelete(string id)
        {
            Product product = await this.productsService.GetDeletedProductsByIdAsyncNoTracking(id);
            if (product == null)
            {
                return this.RedirectToAction("NoProductFound", "Products");
            }

            DetailsProductViewModel productDetailsModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);
            return this.View(productDetailsModel);
        }
    }
}
