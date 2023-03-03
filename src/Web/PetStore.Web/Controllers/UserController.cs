namespace PetStore.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Razor;
    using PetStore.Data.Models;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Client;

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
            ApplicationUser user = await this.clientService.GetClientByIdAsycn(id);
            user.ClientCard = await this.clientCardService.GetCardByIdAsync(user?.ClientCardId);
            if (user == null)
            {
                return this.View("NoClientFound");
            }

            UserViewModel model = AutoMapperConfig.MapperInstance.Map<UserViewModel>(user);

            return this.View(model);
        }
    }
}
