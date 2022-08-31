namespace PetStore.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using PetStore.Data.Models;
    using PetStore.Services.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
