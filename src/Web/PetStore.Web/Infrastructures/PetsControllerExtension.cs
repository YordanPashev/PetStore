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
                this.ViewBag.Title = "No Pet Type Found";
                return this.View("NotFound");
            }

            PetsViewModel model = new PetsViewModel()
            {
                ListOfPets = this.petsService.GetAllPetsForSelectedType(typeName).To<PetDetailsViewModel>().ToArray(),
                PetTypeName = typeName,
            };

            return this.View("Index", model);
        }

        public async Task<IActionResult> EditAndRedirectOrReturnInvalidInputMessage(EditPetViewModel userInputModel)
        {
            PetType petType;
            if (!Enum.TryParse<PetType>(userInputModel.TypeName, out petType))
            {
                return this.RedirectToAction("Edit", new { id = userInputModel.Id, message = GlobalConstants.InvalidDataErrorMessage });
            }

            Pet pet = await this.petsService.GetPetByIdForEditAsync(userInputModel.Id);

            if (!this.ModelState.IsValid || pet == null)
            {
                return this.RedirectToAction("Edit", new { id = userInputModel.Id, message = GlobalConstants.InvalidDataErrorMessage });
            }

            if (!this.IsPetEdited(userInputModel, pet))
            {
                return this.RedirectToAction("Edit", new { id = userInputModel.Id, message = GlobalConstants.EditMessage });
            }

            await this.petsService.UpdatePetDataAsync(userInputModel, pet, petType);
            return this.RedirectToAction("Details", new { id = userInputModel.Id, message = GlobalConstants.SuccessfullyEditProductMessage });
        }

        public IActionResult ViewOrNoPetTypesFound()
        {
            List<PetTypeViewModel> model = new List<PetTypeViewModel>();

            List<string> petTypesNames = Enum.GetNames(typeof(PetType)).Cast<string>().ToList();

            if (petTypesNames == null || petTypesNames.Count == 0)
            {
                this.ViewBag.Title = "There is no Pet Types";
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
                this.ViewBag.Title = "No Pets Found";
                return this.View("NotFound");
            }

            return this.View(listOfPetsModel);
        }

        private bool IsPetEdited(EditPetViewModel userInputModel, Pet pet)
        {
            if (userInputModel.Name == pet.Name && userInputModel.Breed == pet.Breed &&
                userInputModel.BirthDate == pet.BirthDate && userInputModel.TypeName == pet.Type.ToString() &&
                userInputModel.Price == pet.Price && userInputModel.ImageUrl == pet.ImageUrl)
            {
                return false;
            }

            return true;
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
