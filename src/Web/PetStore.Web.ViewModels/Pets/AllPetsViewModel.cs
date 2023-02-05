namespace PetStore.Web.ViewModels.Pets
{
    using System.Collections.Generic;

    public class AllPetsViewModel
    {
        public AllPetsViewModel()
            => this.ListOfAllPets = new HashSet<PetViewModel>();

        public ICollection<PetViewModel> ListOfAllPets { get; set; }

        public string SearchQuery { get; set; }
    }
}
