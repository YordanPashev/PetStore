namespace PetStore.Web.Controllers
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Appointment;
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

            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                this.ViewBag.Message = GlobalConstants.AdminCantMakepetAppointmentsMessage;

                return this.View("NotFound");
            }

            MakeAnPetAppointmentViewModel model = await this.GreateMakeAnPetAppointmentViewModel(petId);

            if (model.Pet == null)
            {
                this.ViewBag.Message = "No Pet found!";

                return this.View("NotFound");
            }

            this.ViewBag.UserErrorMessage = userErrorMessage;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MakeAnAppointment(MakeAnPetAppointmentViewModel model)
        {
            Pet pet = await this.petService.GetPetByIdAsync(model.PetId);

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

            if (!this.CheckIfAppointmentDateIsInWorkingDays(model.Appointment))
            {
                string userErrorMessage = GlobalConstants.ShopsInOpenedFromMessage;

                return this.RedirectToAction("MakeAnAppointment", "PetAppointments", new { area = string.Empty, pet.Id, userErrorMessage });
            }

            await this.petAppointmentService.AddPetAppointmentToDb(model);

            string message = new StringBuilder("You made an appointment to meet ")
                                    .Append(pet.Name)
                                    .Append(" on ")
                                    .Append(model.Appointment.ToString("dddd, dd MMMM yyyy h:mm tt"))
                                    .Append('.')
                                    .ToString();

            return this.RedirectToAction("Details", "Pets", new { area = string.Empty, pet.Id, message });
        }

        private bool CheckIfAppointmentDateIsInWorkingDays(DateTime appointment)
        {
            if (appointment.DayOfWeek == DayOfWeek.Sunday || appointment.Hour < 9 || appointment.Hour > 18)
            {
                return false;
            }

            return true;
        }

        private async Task<MakeAnPetAppointmentViewModel> GreateMakeAnPetAppointmentViewModel(string petId)
        {
            Pet pet = await this.petService.GetPetByIdAsync(petId);

            MakeAnPetAppointmentViewModel model = new MakeAnPetAppointmentViewModel();

            if (pet != null)
            {
                model.Pet = AutoMapperConfig.MapperInstance.Map<PetDetailsViewModel>(pet);
            }

            if (this.User.Identity.IsAuthenticated)
            {
                ApplicationUser user = await this.userManager.GetUserAsync(this.User);

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
