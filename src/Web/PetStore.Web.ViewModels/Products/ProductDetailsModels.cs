namespace PetStore.Web.ViewModels.Products
{
    using AutoMapper;

    using PetStore.Data.Models;
    using PetStore.Services.Mapping;

    public class ProductDetailsModels : IMapFrom<Product>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string CategoryName { get; set; }

        public Category Category { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductDetailsModels>()
                .ForMember(d => d.CategoryName, mo => mo.MapFrom(s => s.Category.Name));
        }
    }
}
