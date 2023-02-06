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

        public IActionResult ViewOrNoPetsFound(AllPetsViewModel model, string message, string search)
        {
            if (model.ListOfAllPets == null || model.ListOfAllPets.Count == 0)
            {
                return this.View("NoPetsFound");
            }

            if (message != null)
            {
                this.ViewBag.UserMessage = message;
            }

            if (!string.IsNullOrEmpty(search))
            {
                model.ListOfAllPets = model.ListOfAllPets.Where(p => p.Name.Contains(search)).ToList();
            }

            return this.View(model);
        }
    }
}
