namespace PetStore.Web.Controllers
{
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
        public async Task<IActionResult> CreateNewOrder(CreateOrderInitialInfoViewModel orderInfo, string userErrorMessage = null)
        {
            if (orderInfo.ProductId == null || orderInfo.Quantity <= 0)
            {
                this.ViewBag.Message = "No Product found or Invalid Quantity!";

                return this.View("NotFound");
            }

            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                string message = GlobalConstants.AdminCantMakeOrdersMessage;
                string id = orderInfo.ProductId;

                return this.RedirectToAction("Details", "Products", new { area = string.Empty, id, message });
            }

            CreateOrderFullInfoViewModel model = await this.CreateOrderDetailsViewModel(orderInfo.ProductId, orderInfo.Quantity);

            this.ViewBag.UserErrorMessage = userErrorMessage;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewOrder(CreateOrderFullInfoViewModel orderDetails)
        {
            if (orderDetails.Quantity <= 0 || orderDetails.ProductId == null || orderDetails.Status != OrderStatus.Pending)
            {
                this.ViewBag.Message = "No Product found or Invalid Quantity!";

                return this.View("NotFound");
            }

            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                string message = GlobalConstants.AdminCantMakeOrdersMessage;
                string id = orderDetails.ProductId;

                return this.RedirectToAction("Details", "Products", new { area = string.Empty, id, message });
            }

            CreateOrderFullInfoViewModel model = await this.CreateOrderDetailsViewModel(orderDetails.ProductId, orderDetails.Quantity, orderDetails);

            if (!this.ModelState.IsValid)
            {
                this.ViewBag.UserErrorMessage = GlobalConstants.InvalidDataErrorMessage;

                return this.View("CreateOrder", model);
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
                string message = GlobalConstants.AdminCantMakeOrdersMessage;

                return this.RedirectToAction("Index", "Home", new { area = string.Empty, message });
            }

            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            OrderShortInfoViewModel[] model = this.ordersService.GetAllClientsOrders(user.Id)
                                                                .To<OrderShortInfoViewModel>()
                                                                .ToArray();

            return this.View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> OrderDetails(string id, string userMessage)
        {
            Order order = await this.ordersService.GetOrderByIdAsync(id);

            if (order == null)
            {
                this.ViewBag.UserMessage = "No order found.";

                return this.View("NotFound");
            }

            OrderFullDetailsViewModel model = AutoMapperConfig.MapperInstance.Map<OrderFullDetailsViewModel>(order);
            this.ViewBag.UserMessage = userMessage;

            return this.View(model);
        }

        private async Task<CreateOrderFullInfoViewModel> CreateOrderDetailsViewModel(string productId, int quantity, CreateOrderFullInfoViewModel orderDetails = null)
        {
            Product product = await this.productsService.GetProductByIdAsync(productId);

            CreateOrderFullInfoViewModel model = new CreateOrderFullInfoViewModel()
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
