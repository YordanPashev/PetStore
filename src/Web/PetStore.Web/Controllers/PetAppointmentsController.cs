namespace PetStore.Web.Controllers
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Appointment;
    using PetStore.Web.ViewModels.PetAppointment;
    using PetStore.Web.ViewModels.Pets;

    public class PetAppointmentsController : Controller
    {
        private readonly IPetAppointmentService petAppointmentService;
        private readonly IPetsService petService;
        private readonly UserManager<ApplicationUser> userManager;

        public PetAppointmentsController(IPetAppointmentService petAppointmentService, IPetsService petService, UserManager<ApplicationUser> userManager)
        {
            this.petAppointmentService = petAppointmentService;
            this.petService = petService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> MakeAnAppointment(string id, string userErrorMessage = null)
        {
            string petId = id;

            ApplicationUser user = await this.userManager.GetUserAsync(this.User);

            MakeAnPetAppointmentViewModel model = await this.GreateMakeAnPetAppointmentViewModel(petId, user);

            if (model.Pet == null)
            {
                this.ViewBag.Message = "No Pet found!";

                return this.View("NotFound");
            }

            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                string message = GlobalConstants.AdminCantMakepetAppointmentsMessage;

                return this.RedirectToAction("Details", "Pets", new { area = string.Empty, id, message });
            }

            if (user != null)
            {
                if (await this.CheckIfCleintAlreadyHasAnAppointmenForSelectedPet(user.Id, petId))
                {
                    string message = GlobalConstants.UserAlreadyHasAnAppointmenForSelectedPet;

                    return this.RedirectToAction("Details", "Pets", new { area = string.Empty, id, message });
                }
            }

            this.ViewBag.UserErrorMessage = userErrorMessage;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MakeAnAppointment(MakeAnPetAppointmentViewModel model)
        {
            Pet pet = await this.petService.GetPetByIdAsync(model.PetId, string.Empty);

            if (pet == null)
            {
                this.ViewBag.Message = "No Pet found!";

                return this.View("NotFound");
            }

            if (!this.ModelState.IsValid)
            {
                string userErrorMessage = GlobalConstants.InvalidDataErrorMessage;

                return this.RedirectToAction("MakeAnAppointment", "PetAppointments", new { area = string.Empty, pet.Id, userErrorMessage });
            }

            if (!this.CheckIfAppointmentValid(model.Appointment))
            {
                string userErrorMessage = new StringBuilder("Invalid date and time.")
                                                .Append(GlobalConstants.ShopsInOpenedFromMessage)
                                                .ToString();

                return this.RedirectToAction("MakeAnAppointment", "PetAppointments", new { area = string.Empty, pet.Id, userErrorMessage });
            }

            await this.petAppointmentService.AddPetAppointmentToDb(model);

            string message = this.GetSuccessfulyRegistratedAppointmentMessage(pet.Name, model.Appointment);

            return this.RedirectToAction("Details", "Pets", new { area = string.Empty, pet.Id, message });
        }

        [HttpGet]
        public async Task<IActionResult> ClientPetAppointments(string id, string userErrorMessage = null)
        {
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                string message = GlobalConstants.AdminCantMakepetAppointmentsMessage;

                return this.RedirectToAction("Index", "Home", new { area = string.Empty, message });
            }

            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            PetAppointmentShortInfoViewModel[] model = await this.petAppointmentService.GetAllClientsAppointments(user.Id)
                                                                .To<PetAppointmentShortInfoViewModel>()
                                                                .ToArrayAsync();

            return this.View(model);
        }

        private async Task<bool> CheckIfCleintAlreadyHasAnAppointmenForSelectedPet(string clientId, string petId)
        {
            if (await this.petAppointmentService.DoesClientHasAppointmenForSelectedPet(clientId, petId))
            {
                return true;
            }

            return false;
        }

        private string GetSuccessfulyRegistratedAppointmentMessage(string petName, DateTime appointment)
            => new StringBuilder("You made an appointment to meet ")
                                    .Append(petName)
                                    .Append(" on ")
                                    .Append(appointment.ToString("dddd, dd MMMM yyyy h:mm tt"))
                                    .Append('.')
                                    .ToString();

        private bool CheckIfAppointmentValid(DateTime appointment)
        {
            if (appointment.DayOfWeek == DayOfWeek.Sunday || appointment.Date < DateTime.Now.Date ||
                appointment.Hour < 9 || appointment.Hour > 18)
            {
                return false;
            }

            return true;
        }

        private async Task<MakeAnPetAppointmentViewModel> GreateMakeAnPetAppointmentViewModel(string petId, ApplicationUser user)
        {
            Pet pet = await this.petService.GetPetByIdAsync(petId, string.Empty);

            MakeAnPetAppointmentViewModel model = new MakeAnPetAppointmentViewModel();

            if (pet != null)
            {
                model.Pet = AutoMapperConfig.MapperInstance.Map<PetDetailsViewModel>(pet);
            }

            if (this.User.Identity.IsAuthenticated)
            {
                model.ClientId = user.Id;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.PhoneNumber = user.PhoneNumber;
                model.Email = user.Email;
            }

            return model;
        }
    }
}
