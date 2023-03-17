namespace PetStore.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Categories;

    public class EditManagerController : AdministrationController
    {
        private readonly ICategoriesService categoriesService;

        public EditManagerController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id, string message = null)
        {
            Category category = await this.categoriesService.GetByIdAsync(id);
            if (category == null)
            {
                return this.View("NoCategoryFound");
            }

            EditCategoryViewModel model = AutoMapperConfig.MapperInstance.Map<EditCategoryViewModel>(category);
            this.ViewBag.UserMessage = message;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(EditCategoryViewModel userInputModel)
        {
            Category category = await this.categoriesService.GetByIdAsync(userInputModel.Id);

            if (category == null)
            {
                return this.View("NoCategoryFound");
            }

            if (!this.ModelState.IsValid || !this.categoriesService.IsCategoryEdited(category, userInputModel))
            {
                string message = !this.ModelState.IsValid ? GlobalConstants.InvalidDataErrorMessage : GlobalConstants.EditMessage;

                return this.RedirectToAction("EditCategory", new { id = userInputModel.Id, message });
            }

            await this.categoriesService.UpdateCategoryAsync(category, userInputModel);
            return this.RedirectToAction("Index", "Categories", new { area = string.Empty, message = GlobalConstants.SuccessfullyEditedProductCategoryMessage });
        }
    }
}
