namespace PetStore.Web.Infrastructures
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.Controllers;
    using PetStore.Web.ViewModels.Pets;
    using PetStore.Web.ViewModels.Search;

    public class PetsControllerExtension : BaseController
    {
        private readonly IPetsService petsService;

        public PetsControllerExtension(IPetsService categoriesService)
            => this.petsService = categoriesService;

        public IActionResult ViewOrNoPetsFound(SearchPetViewModel searchModel)
        {
            PetsViewModel listOfPetsModel = new PetsViewModel()
            {
                ListOfPets = this.GetPets(searchModel.PetTypeName, searchModel.SearchQuery),
                PetTypeName = searchModel.PetTypeName,
                SearchQuery = searchModel.SearchQuery,
            };

            if (listOfPetsModel.ListOfPets == null || listOfPetsModel.ListOfPets.Count == 0)
            {
                return this.View("NoPetsFound");
            }

            return this.View(listOfPetsModel);
        }

        private ICollection<PetDetailsViewModel> GetPets(string typeName, string searchQuery)
        {
            if (typeName == null)
            {
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    return this.petsService.GetAllPetsNoTracking()
                                               .To<PetDetailsViewModel>()
                                               .Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()))
                                               .ToArray();
                }

                return this.petsService.GetAllPetsNoTracking().To<PetDetailsViewModel>().ToArray();
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                return this.petsService.GetAllPetsForSelectedType(typeName)
                                           .To<PetDetailsViewModel>()
                                           .Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()))
                                           .ToArray();
            }

            return this.petsService.GetAllPetsForSelectedType(typeName).To<PetDetailsViewModel>().ToArray();
        }
    }
}
