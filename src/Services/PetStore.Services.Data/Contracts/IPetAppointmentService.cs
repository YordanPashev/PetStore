namespace PetStore.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PetStore.Web.ViewModels.Appointment;

    public interface IPetAppointmentService
    {
        Task AddPetAppointmentToDb(MakeAnPetAppointmentViewModel appointmentInfoViewModel);
    }
}
