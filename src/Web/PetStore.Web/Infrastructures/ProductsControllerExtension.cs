namespace PetStore.Web.Infrastructures
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
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

        public IActionResult ViewOrNoProductFound(DetailsProductViewModel model, string message = null)
        {
            if (model == null)
            {
                return this.View("NoProductFound");
            }

            model.UserMessage = message;

            return this.View(model);
        }

        public IActionResult ViewOrNoProductsFound(SearchAndSortProductViewModel searchModel, ListOfProductsViewModel productsShortInfoModel)
        {
            if (productsShortInfoModel == null && string.IsNullOrEmpty(productsShortInfoModel.SearchQuery))
            {
                return this.View("NoProductFound");
            }

            return this.View(productsShortInfoModel);
        }

        public ICollection<ProductShortInfoViewModel> GetProductsInSale(string categoryName, string searchQuery, string orderCriteria)
        {
            string searchQueryCapitalCase = string.IsNullOrEmpty(searchQuery) ? string.Empty : searchQuery.ToUpper();

            if (categoryName == null)
            {
                if (string.IsNullOrEmpty(searchQueryCapitalCase))
                {
                    return this.productsService.GetAllProductsInSale(orderCriteria)
                                               .To<ProductShortInfoViewModel>().ToArray();
                }

                return this.productsService.GetAllSearchedProductsInSale(searchQueryCapitalCase, orderCriteria)
                                           .To<ProductShortInfoViewModel>().ToArray();
            }

            this.ViewBag.CategoryImageURL = this.categoriesService.GetCategoryImageUrl(categoryName);

            if (string.IsNullOrEmpty(searchQueryCapitalCase))
            {
                return this.productsService.GetAllCategoryProductsInSale(categoryName, orderCriteria)
                                           .To<ProductShortInfoViewModel>().ToArray();
            }

            return this.productsService.GetAllSearchedCategoryProductsInSale(searchQueryCapitalCase, categoryName, orderCriteria)
                                       .To<ProductShortInfoViewModel>().ToArray();
        }

        public ICollection<ProductShortInfoViewModel> GetAllDeletedProducts(string searchQueryCapitalCase, string orderCriteria)
        {
            if (string.IsNullOrEmpty(searchQueryCapitalCase))
            {
                return this.productsService.GetAllDeletedProducts(orderCriteria)
                                           .To<ProductShortInfoViewModel>().ToArray();
            }

            return this.productsService.GetAllSearchedProductsOutOfStock(searchQueryCapitalCase, orderCriteria)
                                       .To<ProductShortInfoViewModel>().ToArray();
        }

        public bool IsProductEdited(ProductInfoViewModel model, Product product)
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
