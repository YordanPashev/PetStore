namespace PetStore.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Data.Models.Enums;
    using PetStore.Services.Data;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Categories;
    using PetStore.Web.ViewModels.Pets;
    using PetStore.Web.ViewModels.Products;

    public class CreateController : AdministrationController
    {
        private readonly IProductsService productsService;
        private readonly ICategoriesService categoriesService;
        private readonly IPetsService petsService;

        public CreateController(IProductsService productService, ICategoriesService categoriesService, IPetsService petsService)
        {
            this.productsService = productService;
            this.categoriesService = categoriesService;
            this.petsService = petsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult AddPet(string message = null)
        {
            PetsWithAllPetTypesViewModel createProductModel = new PetsWithAllPetTypesViewModel()
            {
                CreatePetViewModel = new CreatePetViewModel(),
                PetTypes = Enum.GetNames(typeof(PetType)).Cast<string>().ToList(),
                UserMessage = message,
            };

            return this.View(createProductModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddPet(CreatePetViewModel userInputModel)
        {
            bool isPetTypeValid = Enum.IsDefined(typeof(PetType), userInputModel.TypeName);

            if (userInputModel == null || !this.ModelState.IsValid || !isPetTypeValid)
            {
                return this.RedirectToAction("AddPet", "Create", new { message = GlobalConstants.InvalidDataErrorMessage });
            }

            Pet pet = AutoMapperConfig.MapperInstance.Map<Pet>(userInputModel);
            pet.Type = Enum.Parse<PetType>(userInputModel.TypeName);

            if (this.petsService.IsPetExistingInDb(pet))
            {
                return this.RedirectToAction("AddPet", "Create", new { message = GlobalConstants.PetlreadyExistInDbErrorMessage });
            }

            await this.petsService.AddPetAsync(pet);

            return this.RedirectToAction("Details", "Pets", new { area = string.Empty, id = pet.Id, message = GlobalConstants.SuccessfullyAddedPetMessage });
        }

        [HttpGet]
        public IActionResult CreateCategory(string errorMessage = null)
        {
            this.ViewBag.ErrorMessage = errorMessage;
            return this.View("CreateCategory");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(InputCategoryViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("CreateCategory", "Create", new { errorMessage = GlobalConstants.InvalidDataErrorMessage });
            }

            if (this.categoriesService.IsCategoryExistingInDb(model.Name))
            {
                return this.RedirectToAction("CreateCategory", "Create", new { errorMessage = GlobalConstants.ProductCategoryAlreadyExistInDbErrorMessage });
            }

            Category category = AutoMapperConfig.MapperInstance.Map<Category>(model);
            await this.categoriesService.AddCategoryAsync(category);

            return this.RedirectToAction("Index", "Categories", new { message = GlobalConstants.SuccessfullyAddedProducCategoryMessage });
        }

        [HttpGet]
        public IActionResult CreateProduct(string message = null)
        {
            CreateProductViewModel createProductModel = new CreateProductViewModel()
            {
                Categories = this.categoriesService.GetAllCategoriesNoTracking()
                                                   .To<CategoryShortInfoViewModel>()
                                                   .ToArray(),
                UserMessage = message,
            };

            if (createProductModel.Categories == null)
            {
                return this.View("NoCategoryFound");
            }

            return this.View(createProductModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductInfoViewModel userInputModel)
        {
            userInputModel.CategoryId = await this.categoriesService.GetCategoryIdByNameNoTrackingAsync(userInputModel.CategoryName);
            if (userInputModel == null || !this.ModelState.IsValid || userInputModel.CategoryId < 0)
            {
                return this.RedirectToAction("CreateProduct", "Create", new { message = GlobalConstants.InvalidDataErrorMessage });
            }

            if (this.productsService.IsProductExistingInDb(userInputModel.Name))
            {
                return this.RedirectToAction("CreateProduct", "Create", new { message = GlobalConstants.ProductAlreadyExistInDbErrorMessage });
            }

            Product product = AutoMapperConfig.MapperInstance.Map<Product>(userInputModel);
            product.Id = Guid.NewGuid().ToString();
            await this.productsService.AddProductAsync(product);

            return this.RedirectToAction("Details", "Products", new { id = product.Id, message = GlobalConstants.SuccessfullyAddedProductMessage });
        }
    }
}
