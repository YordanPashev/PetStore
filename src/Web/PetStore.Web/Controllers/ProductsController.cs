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
        public IActionResult Index(SearchAndSortProductViewModel searchAndSortModel, string message = null)
        {
            ListOfProductsViewModel productsShortInfoModel = new ListOfProductsViewModel()
            {
                ListOfProducts = this.productsControllerExtension.GetProductsInSale(searchAndSortModel.CategoryName, searchAndSortModel.SearchQuery, searchAndSortModel.OrderCriteria),
                CategoryName = searchAndSortModel.CategoryName,
                SearchQuery = searchAndSortModel.SearchQuery,
                UserMessage = message,
            };

            return this.productsControllerExtension.ViewOrNoProductsFound(searchAndSortModel, productsShortInfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, string message = null)
        {
            Product product = await this.productsService.GetProductByIdAsync(id);
            DetailsProductViewModel model = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);

            return this.productsControllerExtension.ViewOrNoProductFound(model, message);
        }
    }
}
