namespace PetStore.Web.Controllers
{
    
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Services.Data;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Products;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productService)
        {
            this.productsService = productService;
        }

        [HttpGet]
        public IActionResult Products()
        {
            IQueryable allProducts = this.productsService.GetAllProducts();

            AllProductsViewModel viewModel = new AllProductsViewModel()
            {
                AllProducts = allProducts.To<ListAllProductsViewModel>().ToArray(),
            };

            return this.View(viewModel);
        }
    }
}
