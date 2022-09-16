﻿namespace PetStore.Web.ViewModels.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using PetStore.Data.Models;
    using PetStore.Data.Models.Common;
    using PetStore.Services.Mapping;

    public class CreateProductViewModel : IMapTo<Product>, IHaveCustomMappings
    {
        [Required(ErrorMessage = ProductValidationConstants.NameIsRequired)]
        [MinLength(ProductValidationConstants.NameMinLength, ErrorMessage = ProductValidationConstants.NameMinLengthMessage)]
        [MaxLength(ProductValidationConstants.NameMaxLength, ErrorMessage = ProductValidationConstants.NameMaxLengthMessage)]
        public string Name { get; set; }

        [Required]
        [Range(ProductValidationConstants.PriceMinValue, ProductValidationConstants.PriceMaxValue)]
        public decimal Price { get; set; }

        [Required]
        [RegularExpression(ProductValidationConstants.UrlRegex, ErrorMessage = ProductValidationConstants.InvalidUrlMessage)]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = ProductValidationConstants.DescriptionIsRequired)]
        [MinLength(ProductValidationConstants.DescriptionMinLength)]
        [MaxLength(ProductValidationConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public ICollection<CategoryShortInfoViewModel> CategoriesIfo { get; set; }

        public string ErrorMessage { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, DetailsProductViewModel>()
                .ForMember(d => d.CategoryName, mo => mo.MapFrom(s => s.Category.Name));
        }
    }
}
