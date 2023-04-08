namespace PetStore.Web.Infrastructures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        public ICollection<PetDetailsViewModel> GetPets(string typeName, string searchQuery, string orderCriteria)
        {
            string searchQueryCapitalCase = string.IsNullOrEmpty(searchQuery) ? string.Empty : searchQuery.ToUpper();

            if (typeName == null)
            {
                if (!string.IsNullOrEmpty(searchQueryCapitalCase))
                {
                    return this.petsService.GetAllSearchedPetsInSale(searchQueryCapitalCase, orderCriteria)
                                           .To<PetDetailsViewModel>()
                                           .ToArray();
                }

                return this.petsService.GetAllPetsInSale(orderCriteria).To<PetDetailsViewModel>().ToArray();
            }

            if (!string.IsNullOrEmpty(searchQueryCapitalCase))
            {
                return this.petsService.GetAllSearchedTypePetsInSale(typeName, searchQueryCapitalCase, orderCriteria)
                                       .To<PetDetailsViewModel>()
                                       .ToArray();
            }

            return this.petsService.GetAllTypePetsInSale(typeName, orderCriteria).To<PetDetailsViewModel>().ToArray();
        }

        public ICollection<PetDetailsViewModel> GetDeletedPets(string searchQueryCapitalCase, string orderCriteria)
        {
            if (!string.IsNullOrEmpty(searchQueryCapitalCase))
            {
                return this.petsService.GetAllSearchedRemovedPets(searchQueryCapitalCase, orderCriteria)
                                       .To<PetDetailsViewModel>()
                                       .ToArray();
            }

            return this.petsService.GetAllRemovedPets(orderCriteria).To<PetDetailsViewModel>().ToArray();
        }

        public IActionResult ViewOrNonExistentPetType(string typeName, string orderCriteria)
        {
            if (!Enum.IsDefined(typeof(PetType), typeName))
            {
                this.ViewBag.Message = "No Pet Type Found";
                return this.View("NotFound");
            }

            ListOfPetsViewModel model = new ListOfPetsViewModel()
            {
                ListOfPets = this.petsService.GetAllTypePetsInSale(typeName, orderCriteria).To<PetDetailsViewModel>().ToArray(),
                PetTypeName = typeName,
            };

            return this.View("Index", model);
        }

        public IActionResult ViewOrNoPetsFound(ListOfPetsViewModel model)
        {
            if (model == null && string.IsNullOrEmpty(model.SearchQuery))
            {
                this.ViewBag.Message = "No Pets Found";
                return this.View("NotFound");
            }

            return this.View(model);
        }

        public IActionResult ViewOrNoPetFound(PetDetailsViewModel model, string message = null)
        {
            if (model == null)
            {
                this.ViewBag.Message = "No Pet Found";
                return this.View("NotFound");
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
                return this.View("NotFound");
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
