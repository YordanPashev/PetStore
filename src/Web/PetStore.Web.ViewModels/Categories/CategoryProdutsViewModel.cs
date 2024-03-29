﻿namespace PetStore.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using AutoMapper;

    using PetStore.Data.Models;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Products;

    public class CategoryProdutsViewModel : IMapFrom<Category>, IHaveCustomMappings
    {
        public CategoryProdutsViewModel()
            => this.Products = new HashSet<DetailsProductViewModel>();

        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageURL { get; set; }

        public virtual ICollection<DetailsProductViewModel> Products { get; set; }

        public void CreateMappings(IProfileExpression configuration)
            => configuration.CreateMap<Product, DetailsProductViewModel>();
    }
}
