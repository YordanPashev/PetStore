namespace PetStore.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using PetStore.Data.Models;
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
    }
}
