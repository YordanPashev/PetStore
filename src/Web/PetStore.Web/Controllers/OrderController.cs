namespace PetStore.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.CodeAnalysis;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Orders;
    using PetStore.Web.ViewModels.Products;
    using PetStore.Web.ViewModels.User;

    public class OrderController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private IUsersService usersService;
        private IProductsService productsService;
        private IOrdersService ordersService;

        public OrderController(UserManager<ApplicationUser> userManager, IUsersService usersService, IProductsService productsService, IOrdersService ordersService)
        {
            this.userManager = userManager;
            this.usersService = usersService;
            this.productsService = productsService;
            this.ordersService = ordersService;
        }

        public async Task<IActionResult> OrderDetails(OrderInfoViewModel orderInfo, string userErrorMessage = null)
        {
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                this.ViewBag.Message = "Administrators can't make orders.";
                return this.View("NotFound");
            }

            OrderDetailsViewModel model = await this.CreateOrderDetailsViewModel(orderInfo.ProductId, orderInfo.Quantity);

            if (model.ProductId == null || model.Quantity <= 0)
            {
                this.ViewBag.Message = "No Product found or Invalid Quantity!";
                return this.View("NotFound");
            }

            this.ViewBag.UserErrorMessage = userErrorMessage;

            return this.View(model);
        }

        public async Task<IActionResult> CreateNewOrder(OrderDetailsViewModel orderDetails)
        {
            OrderDetailsViewModel model = await this.CreateOrderDetailsViewModel(orderDetails.ProductId, orderDetails.Quantity);

            if (model.Quantity <= 0 || model.ProductId == null)
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

        private async Task<OrderDetailsViewModel> CreateOrderDetailsViewModel(string productId, int quantity)
        {
            Product product = await this.productsService.GetProductByIdAsync(productId);

            OrderDetailsViewModel model = new OrderDetailsViewModel()
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

                model.FirstName = client.FirstName;
                model.LastName = client.LastName;
                model.PhoneNumber = client.PhoneNumber;
                model.Email = client.Email;
                model.Address = client.Address.AddressText;
                model.ClientId = client.Id;
                model.ClientCardDiscount = client.ClientCard.Discount;
            }

            return model;
        }
    }
}
