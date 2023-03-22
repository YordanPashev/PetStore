namespace PetStore.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.User;

    public class UserManagerController : AdministrationController
    {
        private readonly IAdministrationsService administrationService;
        private readonly IUsersService userService;

        public UserManagerController(IAdministrationsService administrationService, IUsersService userService)
        {
            this.administrationService = administrationService;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            UserShortInfoViewModel[] model = this.administrationService.GetAllUsersWithDeleted().To<UserShortInfoViewModel>().ToArray();

            return this.View(model);
        }

        public async Task<IActionResult> AccountDetails(string id)
        {
            UserDetailsViewModel model = await this.userService.GetUserByIdWtihDeactivatedAsycn(id);

            if (model == null)
            {
                return this.View("NoClientFound");
            }

            return this.View(model);
        }

        public async Task<IActionResult> ActivateAcccountResult(string id)
        {
            ApplicationUser user = await this.administrationService.GetInctiveUserByIdForEditAsync(id);

            if (user == null)
            {
                return this.View("NoClientFound");
            }

            string message = this.GetSuccessfullyActivatedAccountMessage(user.Email);

            await this.administrationService.ActivateUserAccount(user);

            return this.RedirectToAction("Index", "Home", new { area = string.Empty, message });
        }

        private string GetSuccessfullyActivatedAccountMessage(string userEmail)
        {
            StringBuilder message = new StringBuilder();
            message.Append("User: ")
                   .Append(userEmail)
                   .Append(" has been activated.");

            return message.ToString();
        }
    }
}
