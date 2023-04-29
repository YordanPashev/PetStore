namespace PetStore.Services.Data.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.Orders;

    public interface IOrdersService
    {
        Task AddOrderAsync(CreateOrderFullInfoViewModel orderInfo);

        Task ChangeOrderStatus(Order order, string status);

        IQueryable<Order> GetAllClientsOrders(string clientId);

        IQueryable<Order> GetAllOrders();

        Task<Order> GetOrderByIdAsync(string id);

        Task<Order> GetOrderByIdForEditAsync(string id);
    }
}
