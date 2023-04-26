namespace PetStore.Services.Data.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.Orders;

    public interface IOrdersService
    {
        Task AddOrderAsync(CreateOrderViewModel orderInfo);

        IQueryable<Order> GetAllClientsOrders(string clientId);

        IQueryable<Order> GetAllOrders();
    }
}
