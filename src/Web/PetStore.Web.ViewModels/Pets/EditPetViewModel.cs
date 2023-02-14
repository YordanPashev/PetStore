namespace PetStore.Web.ViewModels.Products
{
    using System.Collections.Generic;

    using PetStore.Data.Models;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Pets;

    public class EditPetViewModel : CreatePetViewModel, IMapFrom<Pet>, IMapTo<Pet>
    {
        public EditPetViewModel() => this.PetTypes = new HashSet<string>();

        public string Id { get; set; }

        public ICollection<string> PetTypes { get; set; }

        public string UserMessage { get; set; }
    }
}
