namespace PetStore.Web.Areas.Administration.Controllers
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Microsoft.EntityFrameworkCore;
    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Data.Models.Enums;
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

        public async Task<IActionResult> ChangeOrderStatus(OrderFullDetailsViewModel model)
        {
            Order order = await this.ordersService.GetOrderByIdForEditAsync(model.Id);

            if (order == null || !Enum.IsDefined(typeof(OrderStatus), model.Status))
            {
                this.ViewBag.Message = "Invalid Data. No order found.";
                return this.View("NotFound");
            }

            string userMessage = string.Empty;

            if (model.Status == order.Status.ToString())
            {
                userMessage = new StringBuilder("The status of the order is ")
                                                            .Append(model.Status)
                                                            .Append('.')
                                                            .Append(" If you want to change the status please choose a different one.")
                                                            .ToString();
            }
            else
            {
                await this.ordersService.ChangeOrderStatus(order, model.Status);

                userMessage = new StringBuilder(GlobalConstants.SuccessfullyChangedOrderStatus)
                                                            .Append(model.Status)
                                                            .Append('.')
                                                            .ToString();
            }

            return this.RedirectToAction("OrderDetails", "Orders", new { area = string.Empty, id = model.Id, userMessage });
        }
    }
}
