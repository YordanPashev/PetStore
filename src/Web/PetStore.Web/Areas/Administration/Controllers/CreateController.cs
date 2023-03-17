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
    using PetStore.Web.Infrastructures.Administration;
    using PetStore.Web.ViewModels.Categories;
    using PetStore.Web.ViewModels.Pets;
    using PetStore.Web.ViewModels.Products;

    public class CreateController : AdministrationController
    {
        private readonly ICategoriesService categoriesService;
        private readonly CreateControllerExtension createControllerExtension;

        public CreateController(IProductsService productService, ICategoriesService categoriesService, IPetsService petsService)
        {
            this.categoriesService = categoriesService;
            this.createControllerExtension = new CreateControllerExtension(productService, petsService);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
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

            return this.createControllerExtension.ViewOrNoGategoryFound(createProductModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductInfoViewModel userInputModel)
        {
            userInputModel.CategoryId = await this.categoriesService.GetCategoryIdByNameNoTrackingAsync(userInputModel.CategoryName);
            if (userInputModel == null || !this.ModelState.IsValid || userInputModel.CategoryId < 0)
            {
                return this.RedirectToAction("CreateProduct", new { message = GlobalConstants.InvalidDataErrorMessage });
            }

            return await this.createControllerExtension.CreateProductAndRedirectOrReturnInvalidInputMessage(userInputModel);
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
        public async Task<IActionResult> AddPet(CreatePetViewModel petModel)
        {
            bool isPetTypeValid = Enum.IsDefined(typeof(PetType), petModel.TypeName);

            if (petModel == null || !this.ModelState.IsValid || !isPetTypeValid)
            {
                return this.RedirectToAction("AddPet", new { message = GlobalConstants.InvalidDataErrorMessage });
            }

            PetType petType = Enum.Parse<PetType>(petModel.TypeName);

            return await this.createControllerExtension.CreatePetOrReturnInvalidInputMessage(petModel, petType);
        }
    }
}
