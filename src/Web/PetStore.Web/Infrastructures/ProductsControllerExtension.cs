namespace PetStore.Web.Infrastructures
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Services.Mapping;
    using PetStore.Web.Controllers;
    using PetStore.Web.ViewModels.Products;
    using PetStore.Web.ViewModels.Search;

    public class ProductsControllerExtension : BaseController
    {
        private readonly IProductsService productsService;
        private readonly ICategoriesService categoriesService;

        public ProductsControllerExtension(IProductsService productService, ICategoriesService categoriesService)
        {
            this.productsService = productService;
            this.categoriesService = categoriesService;
        }

        public async Task<IActionResult> EditAndRedirectOrReturnInvalidInputMessage(ProductInfoViewModel userInputModel, Product product = null)
        {
            if (!this.IsProductEdited(userInputModel, product))
            {
                return this.RedirectToAction("Edit", new { id = userInputModel.Id, message = GlobalConstants.EditMessage });
            }

            await this.productsService.UpdateProductDataAsync(userInputModel, product);
            return this.RedirectToAction("Details", new { id = userInputModel.Id, message = GlobalConstants.SuccessfullyEditProductMessage });
        }

        public IActionResult ViewOrNoProductsFound(Product product)
        {
            if (product == null)
            {
                return this.View("NoProductFound");
            }

            DetailsProductViewModel productDetailsModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);

            return this.View(productDetailsModel);
        }

        public IActionResult DeletedProductsViewOrNoProductsFound(string searchQuery)
        {
            ListOfProductsViewModel productsShortInfoModel = new ListOfProductsViewModel()
            {
                ListOfProducts = this.GetDeletedProducts(searchQuery),
                SearchQuery = searchQuery,
            };

            if (productsShortInfoModel == null)
            {
                return this.View("NoProductFound");
            }

            return this.View(productsShortInfoModel);
        }

        public IActionResult ViewOrNoProductsFound(SearchProductViewModel searchModel)
        {
            ListOfProductsViewModel productsShortInfoModel = new ListOfProductsViewModel()
            {
                ListOfProducts = this.GetProductsInSale(searchModel.CategoryName, searchModel.SearchQuery),
                CategoryName = searchModel.CategoryName,
                SearchQuery = searchModel.SearchQuery,
            };

            if (productsShortInfoModel == null && string.IsNullOrEmpty(productsShortInfoModel.SearchQuery))
            {
                return this.View("NoProductFound");
            }

            return this.View(productsShortInfoModel);
        }

        private ICollection<ProductShortInfoViewModel> GetProductsInSale(string categoryName, string searchQuery)
        {
            if (categoryName == null)
            {
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    return this.productsService.GetAllProductsInSale()
                                               .To<ProductShortInfoViewModel>()
                                               .Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()))
                                               .ToArray();
                }

                return this.productsService.GetAllProductsInSale().To<ProductShortInfoViewModel>().ToArray();
            }

            this.ViewBag.CategoryImageURL = this.categoriesService.GetCategoryImageUrl(categoryName);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                return this.productsService.GetAllProductsInSaleForSelectedCateogry(categoryName)
                                           .To<ProductShortInfoViewModel>()
                                           .Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()))
                                           .ToArray();
            }

            return this.productsService.GetAllProductsInSaleForSelectedCateogry(categoryName).To<ProductShortInfoViewModel>().ToArray();
        }

        private ICollection<ProductShortInfoViewModel> GetDeletedProducts(string searchQuery)
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                return this.productsService.GetDeletedProductsNoTracking()
                                           .To<ProductShortInfoViewModel>()
                                           .Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()))
                                           .ToArray();
            }

            return this.productsService.GetDeletedProductsNoTracking().To<ProductShortInfoViewModel>().ToArray();
        }

        private bool IsProductEdited(ProductInfoViewModel model, Product product)
        {
            if (product.Name == model.Name && product.Price == model.Price && product.Description == model.Description &&
                product.ImageUrl == model.ImageUrl && product.CategoryId == model.CategoryId)
            {
                return false;
            }

            return true;
        }
    }
}
