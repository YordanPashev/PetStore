﻿namespace PetStore.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.Infrastructures;
    using PetStore.Web.ViewModels.Pets;
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

        public IActionResult Index(SearchAndSortPetViewModel searchAndSortModel, string message = null)
        {
            ListOfPetsViewModel model = new ListOfPetsViewModel()
            {
                ListOfPets = this.petsControllerExtension.GetPets(searchAndSortModel.PetTypeName, searchAndSortModel.SearchQuery, searchAndSortModel.OrderCriteria),
                PetTypeName = searchAndSortModel.PetTypeName,
                SearchQuery = searchAndSortModel.SearchQuery,
                UserMessage = message,
            };

            return this.petsControllerExtension.ViewOrNoPetsFound(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, string message = null)
        {
            Pet pet = await this.petsService.GetPetByIdAsync(id);
            PetDetailsViewModel model = AutoMapperConfig.MapperInstance.Map<PetDetailsViewModel>(pet);

            return this.petsControllerExtension.ViewOrNoPetFound(model, message);
        }

        [HttpGet]
        public IActionResult PetTypes()
            => this.petsControllerExtension.ViewOrNoPetTypesFound();

        [HttpGet]
        public IActionResult TypePets(string name = null, string orderByCriteria = null)
            => this.petsControllerExtension.ViewOrNonExistentPetType(name, orderByCriteria);
    }
}
