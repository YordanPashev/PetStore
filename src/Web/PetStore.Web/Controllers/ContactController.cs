namespace PetStore.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class ContactController : BaseController
    {
        public IActionResult Index()
            => this.View();

        [HttpGet]
        public async Task<IActionResult> EditPet()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> EditPet(string ss)
        {
            return this.View();
        }
    }
}
