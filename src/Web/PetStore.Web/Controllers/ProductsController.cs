namespace PetStore.Web.Controllers
{
    using System;
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
            IQueryable<Product> allProducts = this.productsService.GetAllProducts().OrderBy(c => c.Name);
            if (allProducts == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            AllProductsViewModel products = new AllProductsViewModel()
            {
                AllProducts = allProducts.To<ProductDetailsViewModels>().ToArray(),
            };

            return this.View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Product product = await this.productsService.GetByIdAsync(id);
            if (product == null)
            {
                return this.RedirectToAction("NoProductFound", "Products");
            }

            ProductDetailsViewModels productDetails = AutoMapperConfig.MapperInstance.Map<ProductDetailsViewModels>(product);
            return this.View(productDetails);
        }

        [HttpGet]
        public IActionResult NoProductFound()
            => this.View();

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create()
        {
            ICollection<ListCategoriesOnProductCreateViewModel> allCategories =
                this.categoriesService.GetAllCategoriesNoTracking().To<ListCategoriesOnProductCreateViewModel>().ToArray();

            if (allCategories == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.View(allCategories);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Create(ProductInputViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Create", "Products");
            }

            Category category = await this.categoriesService.GetByIdAsync(model.CategoryId);

            if (category == null)
            {
                return this.RedirectToAction("Create", "Products");
            }

            Product product = AutoMapperConfig.MapperInstance.Map<Product>(model);
            await this.productsService.AddProductAsync(product);
            model.CategoryName = category.Name;

            return this.RedirectToAction("SuccessfullyAddedProduct", "Products", new RouteValueDictionary(model));
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult SuccessfullyAddedProduct(ProductInputViewModel model)
        {
            return this.View(model);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(string id)
        {
            Product product = await this.productsService.GetByIdAsync(id);

            if (product == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            ProductDetailsViewModels productDetails = AutoMapperConfig.MapperInstance.Map<ProductDetailsViewModels>(product);
            return this.View(productDetails);
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

            ProductDetailsViewModels productDetails = AutoMapperConfig.MapperInstance.Map<ProductDetailsViewModels>(product);
            await this.productsService.DeleteProductAsync(product);

            return this.View(productDetails);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(string id)
        {
            ICollection<ListCategoriesOnProductCreateViewModel> allCategories =
                this.categoriesService.GetAllCategoriesNoTracking().To<ListCategoriesOnProductCreateViewModel>().ToArray();
            Product product = await this.productsService.GetByIdForEditAsync(id);

            if (allCategories == null || product == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            ProductEditViewModel productModel = AutoMapperConfig.MapperInstance.Map<ProductEditViewModel>(product);

            EditProductAndAllCategoriesViewModel model = new EditProductAndAllCategoriesViewModel()
            {
                Product = productModel,
                Categories = allCategories,
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(ProductEditViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("AllProducts", "Products", new RouteValueDictionary(model));
            }

            Category category = await this.categoriesService.GetByIdAsync(model.CategoryId);
            if (category == null)
            {
                return this.RedirectToAction("AllProducts", "Products", new RouteValueDictionary(model));
            }

            Product product = await this.productsService.GetByIdForEditAsync(model.Id);
            if (product == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            if (!this.IsProductEdited(model, product, category))
            {
                return this.RedirectToAction("Edit", "Products", new RouteValueDictionary(model));
            }

            await this.productsService.UpdateProductAsync(product, model, category);
            return this.RedirectToAction("SuccessfullyEditedProduct", "Products", new RouteValueDictionary(model));
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult SuccessfullyEditedProduct(ProductEditViewModel model)
        {
            return this.View(model);
        }

        private bool IsProductEdited(ProductEditViewModel model, Product product, Category category)
        {
            if (product.Name == model.Name && product.Price == model.Price &&
                product.Description == model.Description && product.ImageUrl == model.ImageUrl &&
                product.CategoryId == model.CategoryId && product.Category == category)
            {
                return false;
            }

            return true;
        }
    }
}
