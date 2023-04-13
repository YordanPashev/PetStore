namespace PetStore.Web.ViewModels.Products
{
    using System.Collections.Generic;

    using PetStore.Data.Models;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Pets;

    public class EditPetViewModel : CreatePetViewModel, IMapFrom<Pet>, IMapTo<Pet>
    {
        public string Id { get; set; }

        public ICollection<string> PetTypes { get; set; } = new HashSet<string>();

        public ICollection<string> PetGenders { get; set; } = new HashSet<string>();

        public string UserMessage { get; set; }
    }
}
