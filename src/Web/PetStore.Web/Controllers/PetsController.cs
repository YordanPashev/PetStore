namespace PetStore.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Data.Models;
    using PetStore.Data.Models.Enums;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.Infrastructures;
    using PetStore.Web.ViewModels.Pets;

    public class PetsController : BaseController
    {
        private readonly IPetsService petsService;
        private readonly PetsControllerExtension petsControllerExtension;

        public PetsController(IPetsService petsService)
        {
            this.petsService = petsService;
            this.petsControllerExtension = new PetsControllerExtension(petsService);
        }

        public IActionResult Index(string search, string message = null)
        {
            AllPetsViewModel model = new AllPetsViewModel()
            {
                ListOfAllPets = this.petsService.GetAllPetsNoTracking().To<PetViewModel>().ToArray(),
                SearchQuery = search,
            };

            return this.petsControllerExtension.ViewOrNoPetsFound(model, message, search);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, string message = null)
        {
            Pet pet = await this.petsService.GetByIdAsync(id);

            if (pet == null)
            {
                return this.View("NoPetsFound");
            }

            PetViewModel petModel = AutoMapperConfig.MapperInstance.Map<PetViewModel>(pet);
            petModel.UserMessage = message;
            return this.View(petModel);
        }

        [HttpGet]
        public IActionResult PetTypes()
        {
            PetTypeViewModel[] model = this.GetTypes();
            return this.View(model);
        }

        private PetTypeViewModel[] GetTypes()
        {
            List<string> petTypesNames = Enum.GetNames(typeof(PetType)).Cast<string>().ToList();
            Dictionary<string, string> petTypeUrls = new Dictionary<string, string>()
            {
                { "Dogs", "https://media.istockphoto.com/id/1278389684/photo/large-group-of-various-breeds-of-dogs-together-on-a-white-background.jpg?s=612x612&w=0&k=20&c=MONWoLtCAUTJUbWed01JaLSgbBMclRbFCJ4szEK7iS0=" },
                { "Cats", "https://thumbs.dreamstime.com/b/four-cute-cats-20650677.jpg" },
                { "Birds", "https://i.pinimg.com/originals/7a/a3/1b/7aa31be92644e466a338d52e2d7bc224.jpg" },
                { "Fishes", "https://www.worldatlas.com/r/w1200/upload/04/ab/d1/fish-species-tropical.jpg" },
                { "Rodents", "https://www.earlham.ac.uk/sites/default/files/images/articles/Rodents-are-awesome/Rodents-are-awesome-extreme-evolution-feature-hero.jpg" },
            };

            List<PetTypeViewModel> petTypes = new List<PetTypeViewModel>();
            if (petTypeUrls.Count == petTypeUrls.Count)
            {
                foreach (var typeInfo in petTypeUrls)
                {
                    PetTypeViewModel petType = new PetTypeViewModel(typeInfo.Key, typeInfo.Value);
                    petTypes.Add(petType);
                }
            }

            return petTypes.ToArray();
        }
    }
}
