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
    using PetStore.Web.ViewModels.Products;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;
        private readonly ICategoriesService categoriesService;

        public ProductsController(IProductsService productService, ICategoriesService categoriesService)
        {
            this.productsService = productService;
            this.categoriesService = categoriesService;
        }

        [HttpGet]
        public IActionResult AllProducts()
        {
            IQueryable<Product> allProducts = this.productsService.GetAllProducts();
            if (allProducts == null)
            {
                return this.RedirectToAction("NoProductFound", "Products");
            }

            AllProductsViewModel allProductsModel = new AllProductsViewModel()
            {
                AllProducts = allProducts.To<DetailsProductViewModel>().ToArray(),
            };

            return this.View(allProductsModel);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create(string errorMessage = null)
        {
            ICollection<CategoryShortInfoViewModel> allCategoriesInfo =
                this.categoriesService.GetAllCategoriesNoTracking()
                                        .To<CategoryShortInfoViewModel>()
                                        .ToArray();
            CreateProductViewModel createProductModel = new CreateProductViewModel()
            {
                CategoriesIfo = allCategoriesInfo,
                ErrorMessage = errorMessage,
            };

            if (createProductModel == null)
            {
                return this.RedirectToAction("NoCategoryFound", "Categories");
            }

            return this.View(createProductModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Create(InputProductViewModel model)
        {
            Category category = await this.categoriesService.GetByIdAsync(model.CategoryId);
            if (!this.TryValidateModel(model, nameof(InputProductViewModel)))
            {
                return this.RedirectToAction("Create", "Products", new { errorMessage = ValidationMessages.InvalidData });
            }

            if (category == null)
            {
                return this.RedirectToAction("Create", "Products", new { errorMessage = ValidationMessages.CategoryNotFound });
            }

            if (this.productsService.IsProductExistingInDb(model.Name))
            {
                return this.RedirectToAction("Create", "Products", new { errorMessage = ValidationMessages.ProductAlreadyExistInDb });
            }

            Product product = AutoMapperConfig.MapperInstance.Map<Product>(model);
            await this.productsService.AddProductAsync(product);
            model.CategoryName = category.Name;

            return this.RedirectToAction("SuccessfullyAddedProduct", "Products", new RouteValueDictionary(model));
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

            EditFullInfoViewModel edinPorudctModel = new EditFullInfoViewModel()
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
        public IActionResult SuccessfullyAddedProduct(InputProductViewModel model) => this.View(model);

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
