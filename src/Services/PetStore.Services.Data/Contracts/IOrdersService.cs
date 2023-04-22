namespace PetStore.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PetStore.Data.Models;
    using PetStore.Web.ViewModels.Orders;

    public interface IOrdersService
    {
        Task AddOrderAsync(OrderDetailsViewModel orderInfo);
    }
}
