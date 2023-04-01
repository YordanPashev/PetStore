namespace PetStore.Web.ViewModels.Pets
{
    using System.Collections.Generic;

    using PetStore.Common;

    public class ListOfPetsViewModel
    {
        public ListOfPetsViewModel() => this.ListOfPets = new HashSet<PetDetailsViewModel>();

        public ICollection<PetDetailsViewModel> ListOfPets { get; set; }

        public string PetTypeName { get; set; }

        public string PetTypePlural => PetTypesHelper.GetPetTypePlural(this.PetTypeName);

        public string SearchQuery { get; set; }
    }
}
