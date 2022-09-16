namespace PetStore.Web.Infrastructures
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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
        private readonly ICategoriesService categoriesService;

        public ProductsControllerExtension(IProductsService productService, ICategoriesService categoriesService)
        {
            this.productsService = productService;
            this.categoriesService = categoriesService;
        }

        public IActionResult ViewOrNoProductsFound(IQueryable<Product> allProducts)
        {
            if (allProducts == null)
            {
                return this.RedirectToAction("NoProductFound", "Products");
            }

            AllProductsViewModel allProductsModel = new AllProductsViewModel()
            {
                AllProducts = allProducts.To<DetailsProductViewModel>().ToArray(),
            };

            return this.View(allProductsModel);
        }

        public IActionResult ViewOrNoGategoryFound(CreateProductViewModel createProductModel)
        {
            ICollection<CategoryShortInfoViewModel> allCategoriesInfo =
                this.categoriesService.GetAllCategoriesNoTracking()
                                        .To<CategoryShortInfoViewModel>()
                                        .ToArray();

            if (allCategoriesInfo == null)
            {
                return this.RedirectToAction("NoCategoryFound", "Categories");
            }

            createProductModel.CategoriesIfo = allCategoriesInfo;
            return this.View(createProductModel);
        }

        public async Task<IActionResult> SuccessfulyAddedProdcutOrInvalidData(CreateProductViewModel userInputModel)
        {
            Category category = await this.categoriesService.GetByIdAsync(userInputModel.CategoryId);
            CategoryShortInfoViewModel modelCategory = AutoMapperConfig.MapperInstance.Map<CategoryShortInfoViewModel>(category);

            if (!this.IsModelValid(userInputModel))
            {
                userInputModel.ErrorMessage = GlobalConstants.InvalidDataErrorMessage;
                return this.RedirectToAction("Create", "Products", userInputModel);
            }

            if (modelCategory == null)
            {
                userInputModel.ErrorMessage = GlobalConstants.CategoryNotFoundErrorMessage;
                return this.RedirectToAction("Create", "Products", userInputModel);
            }

            if (this.productsService.IsProductExistingInDb(userInputModel.Name))
            {
                userInputModel.ErrorMessage = GlobalConstants.ProductAlreadyExistInDbErrorMessage;
                return this.RedirectToAction("Create", "Products", userInputModel);
            }

            Product product = AutoMapperConfig.MapperInstance.Map<Product>(userInputModel);
            await this.productsService.AddProductAsync(product);
            userInputModel.CategoryName = modelCategory.Name;

            return this.RedirectToAction("SuccessfullyAddedProduct", "Products", userInputModel);
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
