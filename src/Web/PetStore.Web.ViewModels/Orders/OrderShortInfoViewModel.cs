namespace PetStore.Web.ViewModels.Orders
{
    using AutoMapper;
    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Mapping;

    public class OrderShortInfoViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Address { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public string ApplicationUserId { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }

        public string Status { get; set; }

        public string StatusColor => this.GetStatusColor();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, OrderShortInfoViewModel>()
                .ForMember(od => od.Status, o => o.MapFrom(o => o.Status.ToString()));
        }

        private string GetStatusColor()
        {
            if (this.Status == GlobalConstants.OrderStatuses.Dispatched)
            {
                return "darkgoldenrod";
            }
            else if (this.Status == GlobalConstants.OrderStatuses.Delivered)
            {
                return "green";
            }

            return "red";
        }
    }
}
