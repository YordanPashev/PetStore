namespace PetStore.Web.Infrastructures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Data.Models.Enums;
    using PetStore.Services.Data.Contracts;
    using PetStore.Services.Mapping;
    using PetStore.Web.Controllers;
    using PetStore.Web.ViewModels.Pets;
    using PetStore.Web.ViewModels.Products;

    public class PetsControllerExtension : BaseController
    {
        private readonly IPetsService petsService;

        public PetsControllerExtension(IPetsService categoriesService)
            => this.petsService = categoriesService;

        public bool IsPetEdited(EditPetViewModel userInputModel, Pet pet)
        {
            if (userInputModel.Name == pet.Name && userInputModel.Breed == pet.Breed &&
                userInputModel.BirthDate == pet.BirthDate && userInputModel.TypeName == pet.Type.ToString() &&
                userInputModel.Price == pet.Price && userInputModel.ImageUrl == pet.ImageUrl)
            {
                return false;
            }

            return true;
        }

        public ICollection<PetDetailsViewModel> GetPets(string typeName, string searchQuery)
        {
            if (typeName == null)
            {
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    return this.petsService.GetAllPetsInSaleNoTracking()
                                               .To<PetDetailsViewModel>()
                                               .Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()))
                                               .ToArray();
                }

                return this.petsService.GetAllPetsInSaleNoTracking().To<PetDetailsViewModel>().ToArray();
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                return this.petsService.GetAllPetsInSaleForSelectedType(typeName)
                                           .To<PetDetailsViewModel>()
                                           .Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()))
                                           .ToArray();
            }

            return this.petsService.GetAllPetsInSaleForSelectedType(typeName).To<PetDetailsViewModel>().ToArray();
        }

        public ICollection<PetDetailsViewModel> GetDeletedPets(string searchQuery)
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                return this.petsService.GetAllDeletedPetsNoTracking()
                                           .To<PetDetailsViewModel>()
                                           .Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()))
                                           .ToArray();
            }

            return this.petsService.GetAllDeletedPetsNoTracking().To<PetDetailsViewModel>().ToArray();
        }

        public IActionResult ViewOrNonExistentPetType(string typeName)
        {
            if (!Enum.IsDefined(typeof(PetType), typeName))
            {
                this.ViewBag.Message = "No Pet Type Found";
                return this.View("NotFoundMessageForPetsController");
            }

            ListOfPetsViewModel model = new ListOfPetsViewModel()
            {
                ListOfPets = this.petsService.GetAllPetsInSaleForSelectedType(typeName).To<PetDetailsViewModel>().ToArray(),
                PetTypeName = typeName,
            };

            return this.View("Index", model);
        }

        public IActionResult ViewOrNoPetsFound(ListOfPetsViewModel model)
        {
            if (model == null && string.IsNullOrEmpty(model.SearchQuery))
            {
                this.ViewBag.Message = "No Pets Found";
                return this.View("NotFoundMessageForPetsController");
            }

            return this.View(model);
        }

        public IActionResult ViewOrNoPetFound(PetDetailsViewModel model, string message = null)
        {
            if (model == null)
            {
                this.ViewBag.Message = "No Pet Found";
                return this.View("NotFoundMessageForPetsController");
            }

            model.UserMessage = message;

            return this.View(model);
        }

        public IActionResult ViewOrNoPetTypesFound()
        {
            List<PetTypeViewModel> model = new List<PetTypeViewModel>();

            List<string> petTypesNames = Enum.GetNames(typeof(PetType)).Cast<string>().ToList();

            if (petTypesNames == null || petTypesNames.Count == 0)
            {
                this.ViewBag.Message = "There is no Pet Types";
                return this.View("NotFoundMessageForPetsController");
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
    }
}
