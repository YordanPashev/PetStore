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
            string searchQueryCapitalCase = string.IsNullOrEmpty(searchQuery) ? string.Empty : searchQuery.ToUpper();

            if (categoryName == null)
            {
                if (string.IsNullOrEmpty(searchQueryCapitalCase))
                {
                    return this.productsService.GetAllProductsInSale().To<ProductShortInfoViewModel>().ToArray();
                }

                return this.productsService.GetAllSearchedProductsInSale(searchQueryCapitalCase);
            }

            this.ViewBag.CategoryImageURL = this.categoriesService.GetCategoryImageUrl(categoryName);

            if (string.IsNullOrEmpty(searchQueryCapitalCase))
            {
                return this.productsService.GetAllProductsInSaleForSelectedCateogry(categoryName).To<ProductShortInfoViewModel>().ToArray();
            }

            return this.productsService.GetAllSearchedProductsInSaleForSelectedCateogry(searchQueryCapitalCase, categoryName);
        }

        private ICollection<ProductShortInfoViewModel> GetDeletedProducts(string searchQueryCapitalCase)
        {
            if (string.IsNullOrEmpty(searchQueryCapitalCase))
            {
                return this.productsService.GetAllDeletedProductsNoTracking().To<ProductShortInfoViewModel>().ToArray();
            }

            return this.productsService.GetAllSearchedProductsOutOfStockNoTracking(searchQueryCapitalCase);
        }
    }
}
