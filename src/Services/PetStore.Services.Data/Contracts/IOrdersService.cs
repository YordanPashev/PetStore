namespace PetStore.Services.Data.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.Orders;

    public interface IOrdersService
    {
        Task AddOrderAsync(OrderDetailsViewModel orderInfo);

        IQueryable<Order> GetAllClientsOrders(string clientId);
    }
}
