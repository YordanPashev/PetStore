namespace PetStore.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Data.Models;
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
    }
}
