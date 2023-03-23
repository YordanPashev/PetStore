namespace PetStore.Web.Controllers
{
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
        private readonly IUsersService userService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LogoutModel> logger;

        public UserController(IUsersService userService, SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            this.userService = userService;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        public async Task<IActionResult> Index(string id)
        {
            UserDetailsViewModel model = await this.userService.GetActiveUserByIdAsycn(id);
            if (model == null)
            {
                return this.View("NoClientFound");
            }

            return this.View(model);
        }

        public async Task<IActionResult> DeactivateAcccountResult(string id)
        {
            ApplicationUser user = await this.userService.GetActiveUserByIdForEditAsync(id);
            string message;
            if (user != null)
            {
                if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
                {
                    message = this.GetSuccessfullyDeactivatedAccountMessage(user.Email);
                }
                else
                {
                    await this.signInManager.SignOutAsync();
                    this.logger.LogInformation("User logged out.");
                    message = GlobalConstants.SuccessfullyUserDeactivateHisAccountMessage;
                }

                await this.userService.DeactivateUserAccountAsync(user);

                return this.RedirectToAction("Index", "Home", new { message });
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

            ApplicationUser user = await this.userService.GetActiveUserByIdForEditAsync(userInputModel.Id);

            if (!this.IsUserEdited(userInputModel, user) || userInputModel.Id != user.Id)
            {
                return this.RedirectToAction("Edit", new { id = userInputModel.Id, message = GlobalConstants.EditMessage });
            }

            await this.userService.UpdateUserDataAsync(userInputModel, user);

            return this.RedirectToAction("Index", "User", new { id = userInputModel.Id, message = GlobalConstants.SuccessfullyEditProductMessage });
        }

        private string GetSuccessfullyDeactivatedAccountMessage(string userEmail)
        {
            StringBuilder message = new StringBuilder();
            message.Append("User: ")
                          .Append(userEmail)
                          .Append(" has been deactivated.");

            return message.ToString();
        }

        private bool IsUserEdited(EditUserViewModel editModel, ApplicationUser user)
        {
            if (editModel.FirstName == user.FirstName && editModel.LastName == user.LastName &&
                editModel.Email == user.Email && editModel.PhoneNumber == user.PhoneNumber &&
                editModel.AddressText == user.Address.AddressText)
            {
                return false;
            }

            return true;
        }
    }
}
