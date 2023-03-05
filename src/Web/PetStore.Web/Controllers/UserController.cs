namespace PetStore.Web.Controllers
{
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

        public UserController(IUserService clientService, IClientCardService clientCardService, SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            this.userService = clientService;
            this.clientCardService = clientCardService;
            this.signInManager = signInManager;
            this.logger = logger;
            this.controllerExtension = new UserControllerExtension();
        }

        public async Task<IActionResult> Index(string id)
        {
            UserDetailsViewModel model = await this.userService.GetClientByIdAsycn(id);
            if (model == null || model.Address == null || model.ClientCard == null)
            {
                return this.View("NoClientFound");
            }

            return this.View(model);
        }

        public async Task<IActionResult> DeactivateAcccountResult(string id)
        {
            ApplicationUser user = await this.userService.GetUserByIdForEditAsync(id);
            if (user != null)
            {
                await this.userService.DeactivateUserAccountAsync(user);
                await this.signInManager.SignOutAsync();
                this.logger.LogInformation("User logged out.");
                this.ViewBag.Message = GlobalConstants.SuccessfullyDeactivateUserAccountMessage;

                return this.View("~/Views/Home/Index.cshtml");
            }

            return this.View("NoClientFound");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id, string message = null)
        {
            ApplicationUser user = await this.userService.GetUserByIdForEditAsync(id);
            EditUserViewModel model = AutoMapperConfig.MapperInstance.Map<EditUserViewModel>(user);
            if (model == null || model.Address == null)
            {
                return this.View("NoClientFound");
            }

            model.UserMessage = null;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel userInputModel)
        {
            UserDetailsViewModel user = await this.userService.GetClientByIdAsycn(userInputModel.Id);

            if (!this.ModelState.IsValid || user == null)
            {
                return this.RedirectToAction("Edit", new { id = userInputModel.Id, message = GlobalConstants.InvalidDataErrorMessage });
            }

            return this.View(); //await this.controllerExtension.EditAndRedirectOrReturnInvalidInputMessage(userInputModel, user);
        }
    }
}
