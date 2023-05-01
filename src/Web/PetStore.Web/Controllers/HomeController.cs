namespace PetStore.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Web.ViewModels;

    public class HomeController : BaseController
    {
        public IActionResult Index(string message = null)
        {
            this.ViewBag.Message = message;

            return this.View();
        }

        public IActionResult Privacy()
            => this.View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
