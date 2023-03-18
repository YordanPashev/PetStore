namespace PetStore.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Services.Mapping;
    using PetStore.Web.Infrastructures;
    using PetStore.Web.ViewModels.Products;
    using PetStore.Web.ViewModels.Search;

    public class DeleteController : AdministrationController
    {
        private readonly IProductsService productsService;
        private readonly ProductsControllerExtension productsControllerExtension;

        public DeleteController(IProductsService productService, ICategoriesService categoriesService)
        {
            this.productsService = productService;
            this.productsControllerExtension = new ProductsControllerExtension(productService, categoriesService);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteResult(string id)
        {
            Product product = await this.productsService.GetByProductIdAsync(id);

            if (product != null)
            {
                await this.productsService.DeleteProductAsync(product);
                this.ViewBag.Message = GlobalConstants.SuccessfullyDeleteProductMessage;

                return this.View("SuccessfulOperationTextMessage");
            }

            return this.View("NoProductFound");
        }

        [HttpGet]
        public IActionResult DeletedProducts(SearchProductViewModel searchModel)
        {
            ListOfProductsViewModel productsShortInfoModel = new ListOfProductsViewModel()
            {
                ListOfProducts = this.productsControllerExtension.GetDeletedProducts(searchModel.SearchQuery),
                SearchQuery = searchModel.SearchQuery,
            };

            return this.productsControllerExtension.ViewOrNoProductsFound(searchModel, productsShortInfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeletedProductDetails(string id)
        {
            DetailsProductViewModel model = await this.productsService.GetDeletedProductByIdAsyncNoTracking(id);

            return this.productsControllerExtension.ViewOrNoProductFound(model);
        }

        [HttpGet]
        public async Task<IActionResult> UndeleteResult(string id)
        {
            Product product = await this.productsService.GetDeletedProductByIdAsync(id);

            if (product != null)
            {
                await this.productsService.UndeleteProductAsync(product);
                this.ViewBag.Message = GlobalConstants.SuccessfullyUndeleteProductMessage;

                return this.View("SuccessfulOperationTextMessage");
            }

            return this.View("NoProductFound");
        }
    }
}
