namespace PetStore.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

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

    public class PetsManagerController : AdministrationController
    {
        private readonly IPetsService petsService;
        private readonly PetsControllerExtension petsControllerExtension;

        public PetsManagerController(IPetsService petsService)
        {
            this.petsService = petsService;
            this.petsControllerExtension = new PetsControllerExtension(petsService);
        }

        [HttpGet]
        public IActionResult DeletedPets(SearchPetViewModel searchModel, string orderByCriteria)
        {
            ListOfPetsViewModel model = new ListOfPetsViewModel()
            {
                ListOfPets = this.petsControllerExtension.GetDeletedPets(searchModel.SearchQuery, orderByCriteria),
                SearchQuery = searchModel.SearchQuery,
            };

            return this.petsControllerExtension.ViewOrNoPetsFound(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeletedPetDetails(string id)
        {
            Pet pet = await this.petsService.GetDeletedPetByIdAsyncNoTracking(id);
            PetDetailsViewModel model = AutoMapperConfig.MapperInstance.Map<PetDetailsViewModel>(pet);

            return this.petsControllerExtension.ViewOrNoPetFound(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPet(string id, string message = null)
        {
            Pet pet = await this.petsService.GetPetByIdForEditAsync(id);

            if (pet == null)
            {
                this.ViewBag.Message = "No Pet Found";
                return this.View("NotFoundMessageForPetsController");
            }

            EditPetViewModel model = AutoMapperConfig.MapperInstance.Map<EditPetViewModel>(pet);
            model.TypeName = pet.Type.ToString();
            model.PetTypes = Enum.GetNames(typeof(PetType)).Cast<string>().ToList();
            model.UserMessage = message;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPet(EditPetViewModel userInputModel)
        {
            Pet pet = await this.petsService.GetPetByIdForEditAsync(userInputModel.Id);
            PetType petType;

            if (!this.ModelState.IsValid || !Enum.TryParse<PetType>(userInputModel.TypeName, out petType) || pet == null)
            {
                return this.RedirectToAction("EditPet", "PetsManager", new { id = userInputModel.Id, message = GlobalConstants.InvalidDataErrorMessage });
            }

            if (!this.petsControllerExtension.IsPetEdited(userInputModel, pet))
            {
                return this.RedirectToAction("EditPet", "PetsManager", new { id = userInputModel.Id, message = GlobalConstants.EditMessage });
            }

            await this.petsService.UpdatePetDataAsync(userInputModel, pet, petType);

            return this.RedirectToAction("Details", "Pets", new { area = string.Empty, id = userInputModel.Id, message = GlobalConstants.SuccessfullyEditProductMessage });
        }

        [HttpGet]
        public async Task<IActionResult> DeletePet(string id)
        {
            Pet pet = await this.petsService.GetPetByIdForEditAsync(id);

            if (pet != null)
            {
                await this.petsService.DeletePetAsync(pet);
                this.ViewBag.Message = GlobalConstants.SuccessfullyDeletePetMessage;

                return this.View("SuccessfulOperationTextMessage");
            }

            return this.RedirectToAction("DeletedPets");
        }

        [HttpGet]
        public async Task<IActionResult> UndeletePet(string id)
        {
            Pet pet = await this.petsService.GetDeletedPetByIdAsync(id);

            if (pet != null)
            {
                await this.petsService.UndeletePetAsync(pet);
                this.ViewBag.Message = GlobalConstants.SuccessfullyUndeletePetMessage;

                return this.View("SuccessfulOperationTextMessage");
            }

            return this.RedirectToAction("DeletedPets");
        }
    }
}
