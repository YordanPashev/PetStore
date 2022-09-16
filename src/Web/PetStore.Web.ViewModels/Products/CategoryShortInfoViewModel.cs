namespace PetStore.Web.ViewModels.Products
{
    using PetStore.Data.Models;
    using PetStore.Services.Mapping;

    public class CategoryShortInfoViewModel : IMapFrom<Category>, IMapTo<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }
    }
}
