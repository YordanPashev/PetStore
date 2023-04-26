namespace PetStore.Web.ViewModels.Orders
{
    using AutoMapper;

    using PetStore.Data.Models;
    using PetStore.Services.Mapping;

    public class OrderFullDetailsViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public string ApplicationUserId { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }

        public string Status { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, OrderShortInfoViewModel>()
                .ForMember(od => od.Status, o => o.MapFrom(o => o.Status.ToString()));
        }
    }
}
