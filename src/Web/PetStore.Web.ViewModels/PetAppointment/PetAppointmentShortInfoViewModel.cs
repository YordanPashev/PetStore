namespace PetStore.Web.ViewModels.PetAppointment
{
    using System;

    using PetStore.Data.Models;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Pets;

    public class PetAppointmentShortInfoViewModel : IMapFrom<PetApppointment>
    {
        public string Id { get; set; }

        public DateTime Appointment { get; set; }

        public PetDetailsViewModel Pet { get; set; }
    }
}
