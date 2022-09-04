﻿namespace PetStore.Web.ViewModels.Products
{
    using System;

    using AutoMapper;

    using PetStore.Data.Models;
    using PetStore.Services.Mapping;

    public class ProductDetailsViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string CategoryName { get; set; }

        public Category Category { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductDetailsViewModel>()
                .ForMember(d => d.CategoryName, mo => mo.MapFrom(s => s.Category.Name))
                .ForMember(d => d.CreatedOn, mo => mo.MapFrom(s => s.CreatedOn))
                .ForMember(d => d.ModifiedOn, mo => mo.MapFrom(s => s.ModifiedOn))
                .ForMember(d => d.DeletedOn, mo => mo.MapFrom(s => s.DeletedOn));
        }
    }
}