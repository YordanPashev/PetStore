namespace PetStore.Web.ViewModels.PetAppointment
{
    using System;

    using PetStore.Data.Models;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Pets;

    public class PetAppointmentDetailsViewModel : IMapFrom<PetApppointment>
    {
        public string ClientId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime Appointment { get; set; }

        public DateTime CreatedOn { get; set; }

        public PetDetailsViewModel Pet { get; set; }
    }
}
