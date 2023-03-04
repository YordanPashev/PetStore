namespace PetStore.Web.Controllers
{
    using System.Security.Principal;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;
    using PetStore.Web.ViewModels.User;

    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService clientService;
        private readonly IClientCardService clientCardService;


        public UserController(IUserService clientService, IClientCardService clientCardService)
        {
            this.clientService = clientService;
            this.clientCardService = clientCardService;
        }

        public async Task<IActionResult> Index(string id)
        {
            UserViewModel model = await this.clientService.GetClientByIdAsycn(id);
            if (model == null)
            {
                return this.View("NoClientFound");
            }

            return this.View(model);
        }

        public async Task<IActionResult> DeactivateAcccountResult(string id)
        {
            ApplicationUser user = await this.clientService.GetUserByIdForEditAsync(id);
            if (user != null)
            {
                await this.clientService.DeactivateUserAccountAsync(user);
                this.ViewBag.Message = GlobalConstants.SuccessfullyDeactivateUserAccountMessage;

                return this.RedirectToPage("/Account/Logout", new { area = "Identity" });
            }

            return this.RedirectToAction("Index");
        }
    }
}
