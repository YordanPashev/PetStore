namespace PetStore.Web.ViewModels.Pets
{
    using System.Collections.Generic;

    public class PetsWithAllPetTypesViewModel
    {
        public CreatePetViewModel CreatePetViewModel { get; set; }

        public ICollection<string> PetTypes { get; set; }

        public string UserMessage { get; set; }
    }
}
