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

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Create(InputProductViewModel model)
        {
            Category category = await this.categoriesService.GetByIdAsync(model.CategoryId);
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Create", "Products", new { errorMessage = ValidationMessages.InvalidData });
            }

            if (category == null)
            {
                return this.RedirectToAction("Create", "Products", new { errorMessage = ValidationMessages.CategoryNotFound });
            }

            if (this.IsProductExistingInDb(model, this.productsService))
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
        public IActionResult Create(string errorMessage)
        {
            ICollection<CategoryShortInfoViewModel> allCategoriesInfo =
                this.categoriesService.GetAllCategoriesNoTracking()
                                        .To<CategoryShortInfoViewModel>()
                                        .ToArray();
            CreateProductViewModel createProductModel = new CreateProductViewModel()
            {
                CategoriesIfo = allCategoriesInfo,
                ProducErrorMessage = errorMessage,
            };

            if (createProductModel == null)
            {
                return this.RedirectToAction("NoCategoryFound", "Categories");
            }

            return this.View(createProductModel);
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
        public async Task<IActionResult> Edit(string id)
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
                return this.RedirectToAction("Error", "Home");
            }

            ProductViewModel productModel = AutoMapperConfig.MapperInstance.Map<ProductViewModel>(product);

            EditProductViewModel edinPorudctModel = new EditProductViewModel()
            {
                Product = productModel,
                Categories = allCategories,
            };

            return this.View(edinPorudctModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            Category category = await this.categoriesService.GetByIdAsync(model.CategoryId);
            Product product = await this.productsService.GetByIdForEditAsync(model.Id);
            if (!this.ModelState.IsValid || category == null ||
                !this.IsProductEdited(model, product, category))
            {
                return this.RedirectToAction("Edit", "Products", new RouteValueDictionary(model));
            }

            if (product == null)
            {
                return this.RedirectToAction("NoProductFound", "Products");
            }

            await this.productsService.UpdateProductAsync(product, model);
            model.CategoryName = category.Name;
            return this.RedirectToAction("SuccessfullyEditedProduct", "Products", new RouteValueDictionary(model));
        }

        [HttpGet]
        public IActionResult NoProductFound() => this.View();

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult SuccessfullyAddedProduct(InputProductViewModel model)
        {
            return this.View(model);
        }

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
        public IActionResult SuccessfullyEditedProduct(ProductViewModel model) => this.View(model);

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

        private bool IsProductEdited(ProductViewModel model, Product product, Category category)
        {
            if (product.Name == model.Name && product.Price == model.Price &&
                product.Description == model.Description && product.ImageUrl == model.ImageUrl &&
                product.CategoryId == model.CategoryId && product.Category == category)
            {
                return false;
            }

            return true;
        }

        private bool IsProductExistingInDb(InputProductViewModel model, IProductsService productsService)
            => this.productsService.GetAllProducts().Any(p => p.Name == model.Name &&
                                                         p.CategoryId == model.CategoryId);
    }
}
