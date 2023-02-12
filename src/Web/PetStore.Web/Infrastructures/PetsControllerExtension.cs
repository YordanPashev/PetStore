namespace PetStore.Web.Infrastructures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using PetStore.Common;
    using PetStore.Data.Models.Enums;
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

        public IActionResult AllPetsForSelectedTypeOrNonExistentPetType(string typeName)
        {
            if (!Enum.IsDefined(typeof(PetType), typeName))
            {
                return this.View("NoPetTypeFound");
            }

            PetsViewModel model = new PetsViewModel()
            {
                ListOfPets = this.petsService.GetAllPetsForSelectedType(typeName).To<PetDetailsViewModel>().ToArray(),
                PetTypeName = typeName,
            };

            return this.View("Index", model);
        }

        public IActionResult ViewOrNoPetTypesFound()
        {
            List<PetTypeViewModel> model = new List<PetTypeViewModel>();

            List<string> petTypesNames = Enum.GetNames(typeof(PetType)).Cast<string>().ToList();

            if (petTypesNames == null || petTypesNames.Count == 0)
            {
                return this.View("NoPetTypeFound");
            }

            Dictionary<string, string> petTypeUrls = new Dictionary<string, string>()
            {
                { "Dog", GlobalConstants.DogTypeImage },
                { "Cat", GlobalConstants.CatTypeImage },
                { "Bird", GlobalConstants.BirdTypeImage },
                { "Fish", GlobalConstants.FishTypeImage },
                { "Rodent", GlobalConstants.RodentTypeImage },
            };

            foreach (var typeName in petTypesNames)
            {
                if (petTypeUrls.ContainsKey(typeName))
                {
                    PetTypeViewModel petType = new PetTypeViewModel(typeName, petTypeUrls[typeName]);
                    model.Add(petType);
                }
            }

            return this.View(model);
        }

        public IActionResult ViewOrNoPetsFound(SearchPetViewModel searchModel)
        {
            PetsViewModel listOfPetsModel = new PetsViewModel()
            {
                ListOfPets = this.GetPets(searchModel.PetTypeName, searchModel.SearchQuery),
                PetTypeName = searchModel.PetTypeName,
                SearchQuery = searchModel.SearchQuery,
            };

            if (listOfPetsModel.ListOfPets == null && string.IsNullOrEmpty(listOfPetsModel.SearchQuery))
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
