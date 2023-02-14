namespace PetStore.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Data.Models.Enums;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.Infrastructures;
    using PetStore.Web.ViewModels.Pets;
    using PetStore.Web.ViewModels.Products;
    using PetStore.Web.ViewModels.Search;

    public class PetsController : BaseController
    {
        private readonly IPetsService petsService;
        private readonly PetsControllerExtension petsControllerExtension;

        public PetsController(IPetsService petsService)
        {
            this.petsService = petsService;
            this.petsControllerExtension = new PetsControllerExtension(petsService);
        }

        public IActionResult Index(SearchPetViewModel model)
        {
            return this.petsControllerExtension.ViewOrNoPetsFound(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, string message = null)
        {
            Pet pet = await this.petsService.GetByIdAsync(id);

            if (pet == null)
            {
                this.ViewBag.Title = "No Pet Found";
                return this.View("NotFound");
            }

            PetDetailsViewModel petModel = AutoMapperConfig.MapperInstance.Map<PetDetailsViewModel>(pet);
            petModel.UserMessage = message;

            return this.View(petModel);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(string id, string message = null)
        {
            Pet pet = await this.petsService.GetPetByIdForEditAsync(id);
            if (pet == null)
            {
                this.ViewBag.Title = "No Pet Found";
                return this.View("NotFound");
            }

            EditPetViewModel model = AutoMapperConfig.MapperInstance.Map<EditPetViewModel>(pet);
            model.TypeName = pet.Type.ToString();
            model.PetTypes = Enum.GetNames(typeof(PetType)).Cast<string>().ToList();
            model.UserMessage = message;

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(EditPetViewModel userInputModel)
        {
            return await this.petsControllerExtension.EditAndRedirectOrReturnInvalidInputMessage(userInputModel);
        }

        [HttpGet]
        public IActionResult PetTypes()
        {
            return this.petsControllerExtension.ViewOrNoPetTypesFound();
        }

        [HttpGet]
        public IActionResult TypePets(string name = null)
        {
            return this.petsControllerExtension.AllPetsForSelectedTypeOrNonExistentPetType(name);
        }
    }
}
