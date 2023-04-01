namespace PetStore.Web.Controllers
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

        public IActionResult Index(SearchPetViewModel searchModel)
        {
            ListOfPetsViewModel model = new ListOfPetsViewModel()
            {
                ListOfPets = this.petsControllerExtension.GetPets(searchModel.PetTypeName, searchModel.SearchQuery, searchModel.OrderCriteria),
                PetTypeName = searchModel.PetTypeName,
                SearchQuery = searchModel.SearchQuery,
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
        {
            return this.petsControllerExtension.ViewOrNoPetTypesFound();
        }

        [HttpGet]
        public IActionResult TypePets(string name = null, string orderByCriteria = null)
        {
            return this.petsControllerExtension.ViewOrNonExistentPetType(name, orderByCriteria);
        }
    }
}
