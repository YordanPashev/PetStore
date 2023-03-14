namespace PetStore.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.User;

    public class UserManagerController : AdministrationController
    {
        private readonly IAdministrationService administrationService;
        private readonly IUserService userService;

        public UserManagerController(IAdministrationService administrationService, IUserService userService)
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
    }
}
