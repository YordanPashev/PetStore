namespace PetStore.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Services.Mapping;
    using PetStore.Web.Infrastructures;
    using PetStore.Web.ViewModels.Products;
    using PetStore.Web.ViewModels.Search;

    public class ProductsManagerController : AdministrationController
    {
        private readonly IProductsService productService;
        private readonly ICategoriesService categoriesService;
        private readonly ProductsControllerExtension productsControllerExtension;

        public ProductsManagerController(IProductsService productService, ICategoriesService categoriesService)
        {
            this.productService = productService;
            this.categoriesService = categoriesService;
            this.productsControllerExtension = new ProductsControllerExtension(productService, categoriesService);
        }

        [HttpGet]
        public IActionResult DeletedProducts(SearchAndSortProductViewModel searchAndSortModel, string message = null)
        {
            ListOfProductsViewModel productsShortInfoModel = new ListOfProductsViewModel()
            {
                ListOfProducts = this.productsControllerExtension.GetAllDeletedProducts(searchAndSortModel.SearchQuery, searchAndSortModel.OrderCriteria),
                SearchQuery = searchAndSortModel.SearchQuery,
                UserMessage = message,
            };

            return this.productsControllerExtension.ViewOrNoProductsFound(searchAndSortModel, productsShortInfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            Product product = await this.productService.GetProductByIdAsync(id);

            if (product != null)
            {
                await this.productService.DeleteProductAsync(product);
                string message = new StringBuilder("Product ")
                                               .Append(product.Name)
                                               .Append(GlobalConstants.SuccessfullyDeletedPetMessage)
                                               .ToString();

                return this.RedirectToAction("Index", "Products", new { area = string.Empty, message });
            }

            this.ViewBag.Message = "No product found";

            return this.View("NotFound");
        }

        [HttpGet]
        public async Task<IActionResult> DeletedProductDetails(string id)
        {
            Product deletedProduct = await this.productService.GetDeletedProductByIdAsyncNoTracking(id);
            DetailsProductViewModel model = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(deletedProduct);

            return this.productsControllerExtension.ViewOrNoProductFound(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(string id, string message = null)
        {
            Product product = await this.productService.GetProductByIdForEditAsync(id);
            if (product == null)
            {
                this.ViewBag.Message = "No product found";

                return this.View("NotFound");
            }

            EditProductViewModel editPorudctModel = new EditProductViewModel()
            {
                ProductInfo = AutoMapperConfig.MapperInstance.Map<ProductInfoViewModel>(product),
                Categories = this.categoriesService.GetAllCategoriesNoTracking().To<CategoryShortInfoViewModel>().ToArray(),
                UserMessage = message,
            };

            return this.View(editPorudctModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductInfoViewModel userInputModel)
        {
            userInputModel.CategoryId = await this.categoriesService.GetCategoryIdByNameNoTrackingAsync(userInputModel.CategoryName);
            Product product = await this.productService.GetProductByIdForEditAsync(userInputModel.Id);

            if (!this.ModelState.IsValid || product == null || userInputModel?.CategoryId < 0)
            {
                return this.RedirectToAction("EditProduct", "ProductsManager", new { id = userInputModel.Id, message = GlobalConstants.InvalidDataErrorMessage });
            }

            if (!this.productsControllerExtension.IsProductEdited(userInputModel, product))
            {
                return this.RedirectToAction("EditProduct", "ProductsManager", new { id = userInputModel.Id, message = GlobalConstants.PleaseMakeYourChangesMessage });
            }

            await this.productService.UpdateProductDataAsync(userInputModel, product);

            return this.RedirectToAction("Details", "Products", new { area = string.Empty, id = userInputModel.Id, message = GlobalConstants.SuccessfullyEditProductMessage });
        }

        [HttpGet]
        public async Task<IActionResult> UndeleteProduct(string id)
        {
            Product product = await this.productService.GetDeletedProductByIdAsync(id);

            if (product != null)
            {
                await this.productService.UndeleteProductAsync(product);
                string message = new StringBuilder("Product ")
                                                .Append(product.Name)
                                                .Append(GlobalConstants.SuccessfullyDeletedPetMessage)
                                                .ToString();

                return this.RedirectToAction("DeletedProducts", new { message });
            }

            this.ViewBag.Message = "No product found";

            return this.View("NotFound");
        }
    }
}
