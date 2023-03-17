namespace PetStore.Web.Infrastructures.Administration
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

        public IActionResult ViewOrNoGategoryFound(CreateProductViewModel createProductModel)
        {
            if (createProductModel.Categories == null)
            {
                return View("NoCategoryFound");
            }

            return View(createProductModel);
        }

        public async Task<IActionResult> CreateProductAndRedirectOrReturnInvalidInputMessage(ProductInfoViewModel userInputModel)
        {
            if (productsService.IsProductExistingInDb(userInputModel.Name))
            {
                return RedirectToAction("CreateProduct", new { message = GlobalConstants.ProductAlreadyExistInDbErrorMessage });
            }

            Product product = AutoMapperConfig.MapperInstance.Map<Product>(userInputModel);
            product.Id = Guid.NewGuid().ToString();
            await productsService.AddProductAsync(product);

            return RedirectToAction("Details", "Products", new { id = product.Id, message = GlobalConstants.SuccessfullyAddedProductMessage });
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

            if (petsService.IsPetExistingInDb(pet))
            {
                return RedirectToAction("Pets", new { message = GlobalConstants.PetlreadyExistInDbErrorMessage });
            }

            await petsService.AddPetAsync(pet);

            return RedirectToAction("Details", "Pets", new { id = pet.Id, message = GlobalConstants.SuccessfullyAddedPetMessage });
        }
    }
}
