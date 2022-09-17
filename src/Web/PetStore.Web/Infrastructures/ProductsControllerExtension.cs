namespace PetStore.Web.Infrastructures
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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
        {
            this.productsService = productService;
        }

        public async Task<IActionResult> SuccessfulOperationOrInvalidData(ProductInfoViewModel userInputModel, string actionName, Product product = null)
        {
            if (!this.IsModelValid(userInputModel) || userInputModel.CategoryId < 0 || userInputModel == null)
            {
                userInputModel.ErrorMessage = GlobalConstants.InvalidDataErrorMessage;
                return this.RedirectToAction(actionName, "Products", userInputModel);
            }

            if (actionName == "Create")
            {
                product = AutoMapperConfig.MapperInstance.Map<Product>(userInputModel);
                product.Id = Guid.NewGuid().ToString();
                await this.productsService.AddProductAsync(product);
                return this.RedirectToAction("SuccessfullyAddedProduct", "Products", userInputModel);
            }

            if (actionName == "Edit")
            {
                await this.productsService.UpdateProductAsync(userInputModel, product);
                return this.RedirectToAction("SuccessfullyEditedProduct", "Products", userInputModel);
            }

            return this.RedirectToAction(actionName, "Products", userInputModel);
        }

        public async Task<ActionResult> ViewOrRedirectToAllProducts(Product product, string action)
        {
            DetailsProductViewModel productDetailsModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);
            if (product != null && action == "delete")
            {
                await this.productsService.DeleteAsync(product);
                return this.View(productDetailsModel);
            }

            if (product != null && action == "undelete")
            {
                await this.productsService.UndeleteAsync(product);
                return this.View(productDetailsModel);
            }

            return this.RedirectToAction("AllProducts", "Products");
        }

        public IActionResult ViewOrNoProductsFound(object allProductsModel)
        {
            if (allProductsModel == null)
            {
                return this.RedirectToAction("NoProductFound", "Products");
            }

            return this.View(allProductsModel);
        }

        public IActionResult ViewOrNoGategoryFound(ProductWithAllCategoriesViewModel createProductModel)
        {
            if (createProductModel.Categories == null)
            {
                return this.RedirectToAction("NoCategoryFound", "Categories");
            }

            return this.View(createProductModel);
        }

        private bool IsModelValid(object model)
        {
            var validator = new System.ComponentModel.DataAnnotations.ValidationContext(model);
            var validationRes = new List<ValidationResult>();
            var result = Validator.TryValidateObject(model, validator, validationRes, true);
            return result;
        }
    }
}
