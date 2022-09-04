namespace PetStore.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using AutoMapper;

    using PetStore.Data.Models;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Products;

    public class CategoryViewModel : IMapFrom<Category>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ProductDetailsViewModel> Products { get; set; }

        public void CreateMappings(IProfileExpression configuration)
            => configuration.CreateMap<Product, ProductDetailsViewModel>();
    }
}
