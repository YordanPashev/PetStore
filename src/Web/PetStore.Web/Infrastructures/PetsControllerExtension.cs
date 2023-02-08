namespace PetStore.Web.Infrastructures
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Services.Data.Contracts;
    using PetStore.Web.Controllers;
    using PetStore.Web.ViewModels.Pets;

    public class PetsControllerExtension : BaseController
    {
        private readonly IPetsService petsService;

        public PetsControllerExtension(IPetsService categoriesService)
            => this.petsService = categoriesService;

        public IActionResult ViewOrNoPetsFound(PetsViewModel model, string message, string search)
        {
            if (model.ListOfPets == null || model.ListOfPets.Count == 0)
            {
                return this.View("NoPetsFound");
            }

            if (message != null)
            {
                this.ViewBag.UserMessage = message;
            }

            if (!string.IsNullOrEmpty(search))
            {
                string searchWordLowerCase = search.ToLower();
                model.ListOfPets = model.ListOfPets.Where(p => p.Name.ToLower().Contains(searchWordLowerCase)).ToList();
            }

            return this.View(model);
        }
    }
}
