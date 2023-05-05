namespace PetStore.Web.Areas.Administration.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Orders;
    using PetStore.Web.ViewModels.PetAppointment;

    public class PetAppointmentsManagerController : AdministrationController
    {
        private readonly IPetAppointmentService petAppointmentService;

        public PetAppointmentsManagerController(IPetAppointmentService petAppointmentService)
            => this.petAppointmentService = petAppointmentService;

        public async Task<IActionResult> AllPetAppointmnets()
        {
            PetAppointmentShortInfoViewModel[] model = await this.petAppointmentService.GetAllAppointments()
                                                                                     .To<PetAppointmentShortInfoViewModel>()
                                                                                     .ToArrayAsync();

            return this.View(model);
        }

        public async Task<IActionResult> PetAppointmentDetails(string id)
        {
            PetApppointment petAppointment = await this.petAppointmentService.GetPetAppointmentByIdAsync(id);

            if (petAppointment == null)
            {
                this.ViewBag.Message = "No Pet Appointment found.";

                return this.View("NotFound");
            }

            PetAppointmentDetailsViewModel model = AutoMapperConfig.MapperInstance.Map<PetAppointmentDetailsViewModel>(petAppointment);

            return this.View(model);
        }
    }
}
