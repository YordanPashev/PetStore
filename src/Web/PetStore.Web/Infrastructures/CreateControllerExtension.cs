namespace PetStore.Web.Infrastructures
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Services.Mapping;
    using PetStore.Web.Controllers;
    using PetStore.Web.ViewModels.Products;

    public class CreateControllerExtension : BaseController
    {
        private readonly IProductsService productsService;

        public CreateControllerExtension(IProductsService productService)
            => this.productsService = productService;

        public IActionResult ViewOrNoGategoryFound(ProductWithAllCategoriesViewModel createProductModel)
        {
            if (createProductModel.Categories == null)
            {
                return this.View("NoCategoryFound");
            }

            return this.View(createProductModel);
        }

        public async Task<IActionResult> CreateAndRedirectOrReturnInvalidInputMessage(ProductInfoViewModel userInputModel)
        {
            if (this.productsService.IsProductExistingInDb(userInputModel.Name))
            {
                return this.RedirectToAction("CreateProduct", new { message = GlobalConstants.ProductAlreadyExistInDbErrorMessage });
            }

            Product product = AutoMapperConfig.MapperInstance.Map<Product>(userInputModel);
            product.Id = this.GenerateId();
            await this.productsService.AddProductAsync(product);

            return this.RedirectToAction("Details", "Products", new { id = product.Id, message = GlobalConstants.SuccessfullyAddedProductMessage });
        }

        private string GenerateId()
            => Guid.NewGuid().ToString();
    }
}
