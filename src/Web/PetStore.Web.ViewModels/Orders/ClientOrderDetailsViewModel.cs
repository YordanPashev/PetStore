namespace PetStore.Web.ViewModels.Orders
{
    using AutoMapper;

    using PetStore.Data.Models;
    using PetStore.Services.Mapping;

    public class ClientOrderDetailsViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public string Address { get; set; }

        public string ProductName { get; set; }

        public string ProductImageUrl { get; set; }

        public string ProductCategoryName { get; set; }

        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, OrderDetailsViewModel>()
                .ForMember(od => od.TotalPriceWithDiscount, o => o.MapFrom(o => o.TotalPrice))
                .ForMember(od => od.ProductName, o => o.MapFrom(o => o.Product.Name))
                .ForMember(od => od.ProductImageUrl, o => o.MapFrom(o => o.Product.ImageUrl))
                .ForMember(od => od.ProductCategoryName, o => o.MapFrom(o => o.Product.Category.Name));
        }
    }
}
