namespace PetStore.Web.Infrastructures
{
    using System;
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
            if (!this.productsService.IsProductEdited(userInputModel, product))
            {
                return this.RedirectToAction("Edit", new { id = userInputModel.Id, message = GlobalConstants.EditMessage });
            }

            await this.productsService.UpdateProductAsync(userInputModel, product);
            return this.RedirectToAction("Details", new { id = userInputModel.Id, message = GlobalConstants.SuccessfullyEditProductMessage });
        }

        public async Task<IActionResult> RedirectToSuccessfulOperationOrNoProductFound(Product product, string action)
        {
            DetailsProductViewModel productDetailsModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);
            if (product != null && action == "Delete")
            {
                await this.productsService.DeleteAsync(product);
                return this.RedirectToAction("SuccessfulOperationTextMessage", new { message = GlobalConstants.SuccessfullyDeleteProductMessage });
            }

            if (product != null && action == "Undelete")
            {
                await this.productsService.UndeleteAsync(product);
                return this.RedirectToAction("SuccessfulOperationTextMessage", new { message = GlobalConstants.SuccessfullyUndeleteProductMessage });
            }

            return this.RedirectToAction("Index");
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

        public IActionResult ViewOrNoProductsFound(SearchProductViewModel searchModel)
        {
            ListOfProductsViewModel productsShortInfoModel = new ListOfProductsViewModel()
            {
                ListOfProducts = this.GetProducts(searchModel.CategoryName, searchModel.Search),
                CategoryName = searchModel.CategoryName,
            };

            if (productsShortInfoModel == null)
            {
                return this.View("NoProductFound");
            }

            return this.View(productsShortInfoModel);
        }

        private ICollection<ProductShortInfoViewModel> GetProducts(string categoryName, string search)
        {
            if (categoryName == null)
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return this.productsService.GetAllProductsInSale()
                                               .To<ProductShortInfoViewModel>()
                                               .Where(p => p.Name.ToLower().Contains(search.ToLower()))
                                               .ToArray();
                }

                return this.productsService.GetAllProductsInSale().To<ProductShortInfoViewModel>().ToArray();
            }

            this.ViewBag.CategoryImageURL = this.categoriesService.GetCategoryImageUrl(categoryName);

            if (!string.IsNullOrEmpty(search))
            {
                return this.productsService.GetAllProductsInSaleForSelectedCateogry(categoryName)
                                           .To<ProductShortInfoViewModel>()
                                           .Where(p => p.Name.ToLower().Contains(search.ToLower()))
                                           .ToArray();
            }

            return this.productsService.GetAllProductsInSaleForSelectedCateogry(categoryName).To<ProductShortInfoViewModel>().ToArray();
        }
    }
}
