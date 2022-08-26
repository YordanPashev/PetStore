namespace PetStore.Web.ViewModels.Products
{
    using PetStore.Data.Models;
    using PetStore.Services.Mapping;

    public class ListAllProductsViewModel : IMapFrom<Product>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string CategoryName { get; set; }
    }
}
