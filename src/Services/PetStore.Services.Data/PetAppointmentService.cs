namespace PetStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> DoesClientHasAppointmenForSelectedPet(string clietnId, string petId)
        {
            PetApppointment appointment = await this.petAppointmentRepo.AllAsNoTracking()
                                   .Include(ap => ap.Pet)
                                   .Where(ap => ap.ClientId == clietnId &&
                                                ap.Pet.Id == petId)
                                   .FirstOrDefaultAsync();

            if (appointment == null)
            {
                return false;
            }

            return true;
        }

        public IQueryable<PetApppointment> GetAllAppointments()
                    => this.petAppointmentRepo.AllAsNoTracking()
                             .Include(ap => ap.Pet)
                             .OrderByDescending(ap => ap.CreatedOn);

        public IQueryable<PetApppointment> GetAllClientsAppointments(string clietnId)
            => this.petAppointmentRepo.AllAsNoTracking()
                             .Where(ap => ap.ClientId == clietnId)
                             .Include(ap => ap.Pet)
                             .OrderByDescending(ap => ap.CreatedOn);

        public async Task<PetApppointment> GetPetAppointmentByIdAsync(string id)
            => await this.petAppointmentRepo.AllAsNoTracking()
                             .Where(ap => ap.Id == id)
                             .Include(ap => ap.Pet)
                             .FirstOrDefaultAsync();
    }
}
