namespace PetStore.Web.Controllers
{
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PetStore.Common;
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
        private readonly UserManager<ApplicationUser> userManager;

        public PetsController(IPetsService petsService, UserManager<ApplicationUser> userManager)
        {
            this.petsService = petsService;
            this.petsControllerExtension = new PetsControllerExtension(petsService);
            this.userManager = userManager;
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
            string userId = string.Empty;

            if (this.User.Identity.IsAuthenticated && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                ApplicationUser user = await this.userManager.GetUserAsync(this.User);
                userId = user.Id;
            }

            Pet pet = await this.petsService.GetPetByIdAsync(id, userId);
            PetDetailsViewModel model = AutoMapperConfig.MapperInstance.Map<PetDetailsViewModel>(pet);

            if (pet.PetApppointments != null && pet.PetApppointments.Count > 0)
            {
                string userPetAppointmentDateTime = pet.PetApppointments.FirstOrDefault().Appointment.ToString(GlobalConstants.PetAppointmentDateFormat);
                model.UserHasAppointmentForThisPetMessage = new StringBuilder(GlobalConstants.UserHasAppointmentForThisPetMessage)
                                                                    .Append(userPetAppointmentDateTime)
                                                                    .Append(".")
                                                                    .ToString();
            }

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
