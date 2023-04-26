namespace PetStore.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Microsoft.EntityFrameworkCore;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Orders;

    public class OrdersManager : AdministrationController
    {
        private readonly IOrdersService ordersService;

        public OrdersManager(IOrdersService ordersService)
            => this.ordersService = ordersService;

        public async Task<IActionResult> AllOrders()
        {
            OrderShortInfoViewModel[] model = await this.ordersService.GetAllOrders()
                                                                          .To<OrderShortInfoViewModel>()
                                                                          .ToArrayAsync();

            return this.View(model);
        }
    }
}
