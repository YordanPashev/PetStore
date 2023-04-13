namespace PetStore.Web.ViewModels.Pets
{
    using System.Collections.Generic;

    public class PetsWithAllPetTypesViewModel
    {
        public CreatePetViewModel CreatePetViewModel { get; set; }

        public ICollection<string> PetTypes { get; set; } = new HashSet<string>();

        public ICollection<string> PetGenders { get; set; } = new HashSet<string>();

        public string UserMessage { get; set; }
    }
}
