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

        public async Task<IActionResult> ProcessAndRedirectOrInvalidData(ProductInfoViewModel userInputModel, string actionName, Product product = null)
        {
            if (!this.IsInputModelValid(userInputModel))
            {
                return this.ReturnUserErrorMessage(userInputModel, actionName);
            }

            if (actionName == "Create")
            {
                if (this.productsService.IsProductExistingInDb(userInputModel.Name))
                {
                    return this.RedirectToAction(actionName, new { message = GlobalConstants.ProductAlreadyExistInDbErrorMessage });
                }

                product = AutoMapperConfig.MapperInstance.Map<Product>(userInputModel);
                product.Id = Guid.NewGuid().ToString();
                await this.productsService.AddProductAsync(product);
                return this.RedirectToAction("Details", new { id = product.Id, message = GlobalConstants.SuccessfullyAddProductMessage });
            }

            if (actionName == "Edit")
            {
                if (!this.productsService.IsProductEdited(userInputModel, product))
                {
                    return this.RedirectToAction(actionName, new { id = userInputModel.Id });
                }

                await this.productsService.UpdateProductAsync(userInputModel, product);
                return this.RedirectToAction("Details", new { id = userInputModel.Id, message = GlobalConstants.SuccessfullyEditProductMessage });
            }

            return this.View("NoProductFound");
        }

        public async Task<IActionResult> ViewOrNoProductFound(Product product, string action)
        {
            DetailsProductViewModel productDetailsModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);
            if (product != null && action == "Delete")
            {
                await this.productsService.DeleteAsync(product);
                return this.RedirectToAction("SuccessfullOperationTextMessage", new { message = GlobalConstants.SuccessfullyDeleteProductMessage });
            }

            if (product != null && action == "Undelete")
            {
                await this.productsService.UndeleteAsync(product);
                return this.RedirectToAction("SuccessfullOperationTextMessage", new { message = GlobalConstants.SuccessfullyUndeleteProductMessage });
            }

            return this.RedirectToAction("Index");
        }

        public IActionResult ViewOrNoProductsFound(object allProductsModel)
        {
            if (allProductsModel == null)
            {
                return this.View("NoProductFound");
            }

            return this.View(allProductsModel);
        }

        public IActionResult ViewOrNoGategoryFound(ProductWithAllCategoriesViewModel createProductModel)
        {
            if (createProductModel.Categories == null)
            {
                return this.View("NoCategoryFound");
            }

            return this.View(createProductModel);
        }

        private IActionResult ReturnUserErrorMessage(ProductInfoViewModel userInputModel, string actionName)
        {
            if (actionName == "Edit")
            {
                return this.RedirectToAction("Edit", new { id = userInputModel.Id, errorMessage = GlobalConstants.InvalidDataErrorMessage });
            }

            return this.RedirectToAction("Create", userInputModel);
        }

        private bool IsInputModelValid(ProductInfoViewModel userInputModel)
            => this.IsModelValid(userInputModel) && userInputModel.CategoryId > 0 &&
               userInputModel != null;

        private bool IsModelValid(object model)
        {
            var validator = new System.ComponentModel.DataAnnotations.ValidationContext(model);
            var validationRes = new List<ValidationResult>();
            var result = Validator.TryValidateObject(model, validator, validationRes, true);
            return result;
        }
    }
}
