namespace PetStore.Web.Controllers
{
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.Infrastructures;
    using PetStore.Web.ViewModels.User;

    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IClientCardService clientCardService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LogoutModel> logger;
        private readonly UserControllerExtension controllerExtension;

        public UserController(IUserService userService, IClientCardService clientCardService, SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            this.userService = userService;
            this.clientCardService = clientCardService;
            this.signInManager = signInManager;
            this.logger = logger;
            this.controllerExtension = new UserControllerExtension(userService);
        }

        public async Task<IActionResult> Index(string id)
        {
            UserDetailsViewModel model = await this.userService.GetUserByIdAsycn(id);
            if (model == null)
            {
                return this.View("NoClientFound");
            }

            return this.View(model);
        }

        public async Task<IActionResult> DeactivateAcccountResult(string id)
        {
            ApplicationUser user = await this.userService.GetActiveUserByIdForEditAsync(id);
            if (user != null)
            {
                if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
                {
                    StringBuilder viewBagMessage = new StringBuilder();
                    viewBagMessage.Append("User: ")
                                  .Append(user.Email)
                                  .Append(" has been deactivated.");

                    this.ViewBag.Message = viewBagMessage.ToString();
                }
                else
                {
                    await this.userService.DeactivateUserAccountAsync(user);
                    await this.signInManager.SignOutAsync();
                    this.logger.LogInformation("User logged out.");
                    this.ViewBag.Message = GlobalConstants.SuccessfullyUserDeactivateHisAccountMessage;
                }

                return this.View("~/Views/Home/Index.cshtml");
            }

            return this.View("NoClientFound");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id, string message = null)
        {
            ApplicationUser user = await this.userService.GetActiveUserByIdForEditAsync(id);

            EditUserViewModel model = AutoMapperConfig.MapperInstance.Map<EditUserViewModel>(user);
            model.AddressText = user.Address.AddressText;

            if (model == null || model.AddressText == null)
            {
                return this.View("NoClientFound");
            }

            model.UserMessage = message;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel userInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Edit", new { id = userInputModel.Id, message = GlobalConstants.InvalidDataErrorMessage });
            }

            return await this.controllerExtension.EditAndRedirectOrReturnInvalidInputMessage(userInputModel);
        }
    }
}
