namespace PetStore.Web.Controllers
{
    using System;
    using System.Collections.Generic;
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
                return this.View("NoPetsFound");
            }

            PetDetailsViewModel petModel = AutoMapperConfig.MapperInstance.Map<PetDetailsViewModel>(pet);
            petModel.UserMessage = message;

            return this.View(petModel);
        }

        [HttpGet]
        public IActionResult PetTypes()
        {
            PetTypeViewModel[] model = this.GetAllPetTypes();
            return this.View(model);
        }

        [HttpGet]
        public IActionResult TypePets(string name = null)
        {
            PetsViewModel model = new PetsViewModel()
            {
                ListOfPets = this.petsService.GetAllPetsForSelectedType(name).To<PetDetailsViewModel>().ToArray(),
                PetTypeName = name,
            };

            return this.View("Index", model);
        }

        private PetTypeViewModel[] GetAllPetTypes()
        {
            List<string> petTypesNames = Enum.GetNames(typeof(PetType)).Cast<string>().ToList();
            Dictionary<string, string> petTypeUrls = new Dictionary<string, string>()
            {
                { "Dog", GlobalConstants.DogTypeImage },
                { "Cat", GlobalConstants.CatTypeImage },
                { "Bird", GlobalConstants.BirdTypeImage },
                { "Fish", GlobalConstants.FishTypeImage },
                { "Rodent", GlobalConstants.RodentTypeImage },
            };

            List<PetTypeViewModel> petTypes = new List<PetTypeViewModel>();

            foreach (var typeName in petTypesNames)
            {
                if (petTypeUrls.ContainsKey(typeName))
                {
                    PetTypeViewModel petType = new PetTypeViewModel(typeName, petTypeUrls[typeName]);
                    petTypes.Add(petType);
                }
            }

            return petTypes.ToArray();
        }
    }
}
