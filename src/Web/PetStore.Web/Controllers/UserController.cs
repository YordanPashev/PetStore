namespace PetStore.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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
    }
}
