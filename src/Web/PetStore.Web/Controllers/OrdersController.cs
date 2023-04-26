namespace PetStore.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Data.Models.Enums;
    using PetStore.Services.Data;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Orders;
    using PetStore.Web.ViewModels.User;

    public class OrdersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private IUsersService usersService;
        private IProductsService productsService;
        private IOrdersService ordersService;

        public OrdersController(UserManager<ApplicationUser> userManager, IUsersService usersService, IProductsService productsService, IOrdersService ordersService)
        {
            this.userManager = userManager;
            this.usersService = usersService;
            this.productsService = productsService;
            this.ordersService = ordersService;
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetails(OrderInfoViewModel orderInfo, string userErrorMessage = null)
        {
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                this.ViewBag.Message = GlobalConstants.AdminCantMakeOrdersMessage;
                return this.View("NotFound");
            }

            CreateOrderViewModel model = await this.CreateOrderDetailsViewModel(orderInfo.ProductId, orderInfo.Quantity);

            if (model.ProductId == null || model.Quantity <= 0)
            {
                this.ViewBag.Message = "No Product found or Invalid Quantity!";
                return this.View("NotFound");
            }

            this.ViewBag.UserErrorMessage = userErrorMessage;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewOrder(CreateOrderViewModel orderDetails)
        {
            CreateOrderViewModel model = await this.CreateOrderDetailsViewModel(orderDetails.ProductId, orderDetails.Quantity, orderDetails);

            if (model.Quantity <= 0 || model.ProductId == null || model.Status != OrderStatus.Pending)
            {
                this.ViewBag.Message = "No Product found or Invalid Quantity!";
                return this.View("NotFound");
            }

            if (!this.ModelState.IsValid)
            {
                this.ViewBag.UserErrorMessage = "Invalid Data!";

                return this.View("OrderDetails", model);
            }

            await this.ordersService.AddOrderAsync(model);

            return this.RedirectToAction("Index", "Home", new { area = string.Empty, message = GlobalConstants.SuccessfullySendedOrder });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ClientOrders()
        {
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                this.ViewBag.Message = GlobalConstants.AdminCantMakeOrdersMessage;
                return this.View("NotFound");
            }

            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            OrderShortInfoViewModel[] model = this.ordersService.GetAllClientsOrders(user.Id)
                                                                .To<OrderShortInfoViewModel>()
                                                                .ToArray();

            return this.View(model);
        }

        private async Task<CreateOrderViewModel> CreateOrderDetailsViewModel(string productId, int quantity, CreateOrderViewModel orderDetails = null)
        {
            Product product = await this.productsService.GetProductByIdAsync(productId);

            CreateOrderViewModel model = new CreateOrderViewModel()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductCategoryName = product.Category.Name,
                ProductImageUrl = product.ImageUrl,
                ProductPrice = product.Price,
                Quantity = quantity,
            };

            if (this.User.Identity.IsAuthenticated)
            {
                ApplicationUser user = await this.userManager.GetUserAsync(this.User);
                UserDetailsViewModel client = await this.usersService.GetActiveUserByIdAsycn(user.Id);
                string address = orderDetails == null ? client.Address.AddressText : orderDetails.Address;

                model.FirstName = client.FirstName;
                model.LastName = client.LastName;
                model.PhoneNumber = client.PhoneNumber;
                model.Email = client.Email;
                model.Address = address;
                model.ClientId = client.Id;
                model.ClientCardDiscount = client.ClientCard.Discount;
            }
            else if (orderDetails != null)
            {
                model.FirstName = orderDetails.FirstName;
                model.LastName = orderDetails.LastName;
                model.PhoneNumber = orderDetails.PhoneNumber;
                model.Email = orderDetails.Email;
                model.Address = orderDetails.Address;
                model.ClientCardDiscount = orderDetails.ClientCardDiscount;
            }

            return model;
        }
    }
}
