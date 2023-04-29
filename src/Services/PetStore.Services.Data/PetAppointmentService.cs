namespace PetStore.Services.Data
{
    using System.Threading.Tasks;

    using PetStore.Data.Common.Repositories;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Appointment;

    public class PetAppointmentService : IPetAppointmentService
    {
        private readonly IDeletableEntityRepository<PetApppointment> petAppointmentRepo;

        public PetAppointmentService(IDeletableEntityRepository<PetApppointment> petAppointmentRepo)
            => this.petAppointmentRepo = petAppointmentRepo;

        public async Task AddPetAppointmentToDb(MakeAnPetAppointmentViewModel appointmentInfoViewModel)
        {
            PetApppointment appointment = AutoMapperConfig.MapperInstance.Map<PetApppointment>(appointmentInfoViewModel);

            await this.petAppointmentRepo.AddAsync(appointment);

            await this.petAppointmentRepo.SaveChangesAsync();
        }
    }
}
