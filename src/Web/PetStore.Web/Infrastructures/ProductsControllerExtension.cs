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

    public class ProductsControllerExtension : BaseController
    {
        private readonly IProductsService productsService;

        public ProductsControllerExtension(IProductsService productService)
            => this.productsService = productService;

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

        public IActionResult ViewOrNoProductsFound(ListOfProductsViewModel allProductsModel, string search)
        {
            if (allProductsModel == null)
            {
                return this.View("NoProductFound");
            }

            if (!string.IsNullOrEmpty(search))
            {
                allProductsModel = new ListOfProductsViewModel()
                {
                    ListOfProducts = this.GetAllMatchingProducts(allProductsModel.ListOfProducts, search),
                    SearchQuery = search,
                };
            }

            return this.View(allProductsModel);
        }

        private ICollection<ProductShortInfoViewModel> GetAllMatchingProducts(ICollection<ProductShortInfoViewModel> allProducts, string search)
            => allProducts.Where(p => p.Name.ToLower().Contains(search.ToLower())).ToArray();
    }
}
