namespace PetStore.Web.ViewModels.Pets
{
    using System.Collections.Generic;

    public class PetsViewModel
    {
        public PetsViewModel() => this.ListOfPets = new HashSet<PetDetailsViewModel>();

        public ICollection<PetDetailsViewModel> ListOfPets { get; set; } = null;

        public string PetTypeName { get; set; }

        public string SearchQuery { get; set; }

    }
}
