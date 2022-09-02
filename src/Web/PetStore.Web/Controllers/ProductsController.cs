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
            IQueryable allProducts = this.productsService.GetAllProducts();

            AllProductsViewModel products = new AllProductsViewModel()
            {
                AllProducts = allProducts.To<ProductModel>().ToArray(),
            };

            return this.View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Product product = await this.productsService.GetById(id);
            if (product == null)
            {
                this.RedirectToAction("Error", "Home");
            }

            ProductDetailsModels productDetails = AutoMapperConfig.MapperInstance.Map<ProductDetailsModels>(product);

            return this.View(productDetails);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create()
        {
            ICollection<ListCategoriesOnProductCreateViewModel> allCategories =
                this.categoriesService.GetAllCategoriesNoTracking().To<ListCategoriesOnProductCreateViewModel>().ToArray();
            return this.View(allCategories);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Create(ProductInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Create", "Products");
            }

            Category cateogry = await this.categoriesService.GetById(model.CategoryId);

            if (cateogry == null)
            {
                return this.RedirectToAction("Create", "Products");
            }

            Category category = await this.categoriesService.GetById(model.CategoryId);
            Product product = AutoMapperConfig.MapperInstance.Map<Product>(model);
            await this.productsService.AddProduct(product);

            model.CategoryName = category.Name;
            return this.RedirectToAction("SuccessfullyAddedProduct", "Products", new RouteValueDictionary(model));
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult SuccessfullyAddedProduct(ProductInputModel model)
        {
            return this.View(model);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(string id)
        {
            Product product = await this.productsService.GetById(id);
            ProductDetailsModels productDetails = AutoMapperConfig.MapperInstance.Map<ProductDetailsModels>(product);
            return this.View(productDetails);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> SuccessfullyDeletedProduct(string id)
        {
            Product product = await this.productsService.GetById(id);

            if (product == null)
            {
                return this.RedirectToAction("AllProducts", "Products");
            }

            ProductDetailsModels productDetails = AutoMapperConfig.MapperInstance.Map<ProductDetailsModels>(product);
            await this.productsService.DeleteProduct(product);

            return this.View(productDetails);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(string id)
        {
            ICollection<ListCategoriesOnProductCreateViewModel> allCategories =
                this.categoriesService.GetAllCategoriesNoTracking().To<ListCategoriesOnProductCreateViewModel>().ToArray();
            Product product = await this.productsService.GetByIdForEdit(id);
            ProductModel productDetails = AutoMapperConfig.MapperInstance.Map<ProductModel>(product);

            ProductEditModel model = new ProductEditModel()
            {
                Product = productDetails,
                Categories = allCategories,
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> SuccessfullyEditedProduct(ProductModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Edit", "Products", new RouteValueDictionary(model));
            }

            Category category = await this.categoriesService.GetById(model.CategoryId);

            if (category == null)
            {
                return this.RedirectToAction("Edit", "Products", new RouteValueDictionary(model));
            }

            Product product = await this.productsService.GetByIdForEdit(model.Id);

            if (!this.IsProductEdited(model, product, category))
            {
                return this.RedirectToAction("Edit", "Products", new RouteValueDictionary(model));
            }

            product.Name = model.Name;
            product.Price = model.Price;
            product.Description = model.Description;
            product.ImageUrl = model.ImageUrl;
            product.CategoryId = model.CategoryId;
            product.Category = category;

            model.Category = category;
            await this.productsService.UpdateProduct(product);
            return this.View(model);
        }

        private bool IsProductEdited(ProductModel model, Product product, Category category)
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
