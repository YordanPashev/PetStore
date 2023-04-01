namespace PetStore.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Services.Mapping;
    using PetStore.Web.Infrastructures;
    using PetStore.Web.ViewModels.Products;
    using PetStore.Web.ViewModels.Search;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;
        private readonly ProductsControllerExtension productsControllerExtension;

        public ProductsController(IProductsService productService, ICategoriesService categoriesService)
        {
            this.productsService = productService;
            this.productsControllerExtension = new ProductsControllerExtension(productService, categoriesService);
        }

        [HttpGet]
        public IActionResult Index(SearchAndSortProductViewModel searchModel)
        {
            ListOfProductsViewModel productsShortInfoModel = new ListOfProductsViewModel()
            {
                ListOfProducts = this.productsControllerExtension.GetProductsInSale(searchModel.CategoryName, searchModel.SearchQuery),
                CategoryName = searchModel.CategoryName,
                SearchQuery = searchModel.SearchQuery,
            };

            return this.productsControllerExtension.ViewOrNoProductsFound(searchModel, productsShortInfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, string message = null)
        {
            Product product = await this.productsService.GetByProductIdAsync(id);
            DetailsProductViewModel model = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);

            return this.productsControllerExtension.ViewOrNoProductFound(model, message);
        }
    }
}
