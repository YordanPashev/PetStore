namespace PetStore.Web.ViewModels.PetAppointment
{
    using System;

    using PetStore.Data.Models;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Pets;

    public class ClientAppointmentInfoViewModel : IMapFrom<PetApppointment>
    {
        public DateTime Appointment { get; set; }

        public PetDetailsViewModel Pet { get; set; }
    }
}
