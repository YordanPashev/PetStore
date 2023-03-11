namespace PetStore.Web.Areas.Administration.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.User;

    public class UserManagerController : AdministrationController
    {
        private readonly IAdministrationService administrationService;

        public UserManagerController(IAdministrationService administrationService)
            => this.administrationService = administrationService;

        public IActionResult Index()
        {
            UserShortInfoViewModel[] model = this.administrationService.GetAllUsers().To<UserShortInfoViewModel>().ToArray();

            return this.View(model);
        }
    }
}
