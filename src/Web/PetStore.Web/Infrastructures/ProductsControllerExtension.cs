﻿namespace PetStore.Web.Infrastructures
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

        public async Task<IActionResult> SuccessfullOperationOrInvalidData(ProductInfoViewModel userInputModel, string actionName, Product product = null)
        {
            if (!this.IsInputModelValid(userInputModel))
            {
                return this.ReturnUserErrorMessage(userInputModel, actionName);
            }

            if (actionName == "Create")
            {
                if (this.productsService.IsProductExistingInDb(userInputModel.Name))
                {
                    userInputModel.ErrorMessage = GlobalConstants.ProductAlreadyExistInDbErrorMessage;
                    return this.View(actionName, userInputModel);
                }

                product = AutoMapperConfig.MapperInstance.Map<Product>(userInputModel);
                product.Id = Guid.NewGuid().ToString();
                await this.productsService.AddProductAsync(product);
                return this.RedirectToAction("SuccessfullyAddedProduct", "Products", userInputModel);
            }

            if (actionName == "Edit")
            {
                if (await this.productsService.GetByIdAsync(userInputModel.Id) == null)
                {
                    return this.View("NoProductFound");
                }

                if (!this.productsService.IsProductEdited(userInputModel, product))
                {
                    return this.RedirectToAction(actionName, new { modelId = userInputModel.Id, errorMessage = GlobalConstants.NothingWasEditedErrorMessage });
                }

                await this.productsService.UpdateProductAsync(userInputModel, product);
                return this.RedirectToAction("SuccessfullyEditedProduct", "Products", userInputModel);
            }

            return this.View(actionName, userInputModel);
        }

        public async Task<IActionResult> ViewOrNoProductFound(Product product, string action)
        {
            DetailsProductViewModel productDetailsModel = AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);
            if (product != null && action == "delete")
            {
                await this.productsService.DeleteAsync(product);
                this.ViewBag.Message = GlobalConstants.SuccessfullyDeleteProductMessage;
                return this.RedirectToAction("SuccessfullOperationTextMessage");
            }

            if (product != null && action == "undelete")
            {
                await this.productsService.UndeleteAsync(product);
                this.ViewBag.Message = GlobalConstants.SuccessfullyUndeleteProductMessage;
                return this.RedirectToAction("SuccessfullOperationTextMessage");
            }

            return this.View("NoProductFound");
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
                return this.View("NoCategoryFound", "Categories");
            }

            return this.View(createProductModel);
        }

        private IActionResult ReturnUserErrorMessage(ProductInfoViewModel userInputModel, string actionName)
        {
            userInputModel.ErrorMessage = GlobalConstants.InvalidDataErrorMessage;

            if (actionName == "Edit")
            {
                return this.RedirectToAction(actionName, "Products", new { id = userInputModel.Id, errorMessage = userInputModel.ErrorMessage });
            }

            return this.View(actionName, userInputModel);
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
