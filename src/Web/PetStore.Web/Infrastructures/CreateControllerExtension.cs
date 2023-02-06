namespace PetStore.Web.Infrastructures
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Data.Models.Enums;
    using PetStore.Services.Data;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.Controllers;
    using PetStore.Web.ViewModels.Pets;
    using PetStore.Web.ViewModels.Products;

    public class CreateControllerExtension : BaseController
    {
        private readonly IProductsService productsService;
        private readonly IPetsService petsService;

        public CreateControllerExtension(IProductsService productService, IPetsService petsService)
        {
            this.productsService = productService;
            this.petsService = petsService;
        }

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
            product.Id = Guid.NewGuid().ToString();
            await this.productsService.AddProductAsync(product);

            return this.RedirectToAction("Details", "Products", new { id = product.Id, message = GlobalConstants.SuccessfullyAddedProductMessage });
        }

        public async Task<IActionResult> CreatePetOrReturnInvalidInputMessage(CreatePetViewModel petModel, PetType petType)
        {
            Pet pet = new Pet()
            {
                Id = Guid.NewGuid().ToString(),
                Name = petModel.Name,
                Breed = petModel.Breed,
                BirthDate = petModel.BirthDate,
                Price = petModel.Price,
                ImageUrl = petModel.ImageUrl,
                Type = petType,
            };

            if (this.petsService.IsPetExistingInDb(pet))
            {
                return this.RedirectToAction("Pets", new { message = GlobalConstants.PetlreadyExistInDbErrorMessage });
            }

            await this.petsService.AddPetAsync(pet);

            return this.RedirectToAction("Details", "Pets", new { id = pet.Id, message = GlobalConstants.SuccessfullyAddedPetMessage});
        }
    }
}
