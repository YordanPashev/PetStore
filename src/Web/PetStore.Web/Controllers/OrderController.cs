namespace PetStore.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Orders;
    using PetStore.Web.ViewModels.Products;

    public class OrderController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private IUsersService usersService;
        private IProductsService productsService;

        public OrderController(UserManager<ApplicationUser> userManager, IUsersService usersService, IProductsService productsService)
        {
            this.userManager = userManager;
            this.usersService = usersService;
            this.productsService = productsService;
        }

        public async Task<IActionResult> OrderDetails(OrderInfoViewModel orderInfo)
        {
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                this.ViewBag.Message = "Administrators can't make orders.";
                return this.View("NotFound");
            }

            Product product = await this.productsService.GetProductByIdAsync(orderInfo.ProductId);

            if (orderInfo.Quantity <= 0 || orderInfo.ProductId == null)
            {
                this.ViewBag.Message = "No Product found or Invalid Quantity!";
                return this.View("NotFound");
            }

            OrderDetailsViewModel model = new OrderDetailsViewModel()
            {
                Product = AutoMapperConfig.MapperInstance.Map<ProductShortInfoViewModel>(product),
                Quantity = orderInfo.Quantity,
            };

            if (this.User.Identity.IsAuthenticated)
            {
                ApplicationUser user = await this.userManager.GetUserAsync(this.User);
                model.Client = await this.usersService.GetActiveUserByIdAsycn(user.Id);
            }

            return this.View(model);
        }
    }
}
