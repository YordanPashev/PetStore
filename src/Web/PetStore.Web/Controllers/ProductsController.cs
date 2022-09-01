namespace PetStore.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Product;
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
        public IActionResult Products()
        {
            IQueryable allProducts = this.productsService.GetAllProducts();

            AllProductsViewModel products = new AllProductsViewModel()
            {
                AllProducts = allProducts.To<ProductViewModel>().ToArray(),
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

            ProductDetailsVieModels productDetails =
                AutoMapperConfig.MapperInstance.Map<ProductDetailsVieModels>(product);

            return this.View(productDetails);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create()
        {
            ICollection<ListCategoriesOnProductCreateViewModel> allCategories =
                this.categoriesService.GetAllCategories().To<ListCategoriesOnProductCreateViewModel>().ToArray();

            return this.View(allCategories);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Create(ProductInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Create", "Product");
            }

            Category cateogry = await this.categoriesService.GetById(model.CategoryId);

            if (cateogry == null)
            {
                return this.RedirectToAction("Create", "Product");
            }

            Product product = AutoMapperConfig.MapperInstance.Map<Product>(model);
            await this.productsService.AddProduct(product);

            return this.View("SuccessfullyAddedItem", model.Name);
        }
    }
}
