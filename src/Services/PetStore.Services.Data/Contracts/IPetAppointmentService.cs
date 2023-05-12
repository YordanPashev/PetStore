namespace PetStore.Services.Data.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.Appointment;

    public interface IPetAppointmentService
    {
        Task AddPetAppointmentToDb(MakeAnPetAppointmentViewModel appointmentInfoViewModel);

        Task<bool> DoesClientHasAppointmenForSelectedPet(string clientId, string petId);

        IQueryable<PetApppointment> GetAllClientsAppointments(string clietnId);

        IQueryable<PetApppointment> GetAllAppointments();

        Task<PetApppointment> GetPetAppointmentByIdAsync(string id);
    }
}
