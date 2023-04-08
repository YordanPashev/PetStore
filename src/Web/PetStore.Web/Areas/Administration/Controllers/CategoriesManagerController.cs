namespace PetStore.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Categories;

    public class CategoriesManagerController : AdministrationController
    {
        private readonly ICategoriesService categoriesService;
        private readonly IProductsService productsService;

        public CategoriesManagerController(ICategoriesService categoriesService, IProductsService productsService)
        {
            this.categoriesService = categoriesService;
            this.productsService = productsService;
        }

        [HttpGet]
        public IActionResult DeletedCategories(string message)
        {
            List<DeletedCategoryViewModel> model = this.categoriesService.GetDeletedCategories().ToList();
            this.ViewBag.Message = message;

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            Category category = await this.categoriesService.GetByIdForEditAsync(id);

            if (category == null)
            {
                this.ViewBag.Message = "No category found";

                return this.View("NotFound");
            }

            if (category.Products.Count > 0)
            {
                return this.RedirectToAction("Index", "Categories", new { area = string.Empty, message = GlobalConstants.CantDeleteCateoryWithProductsMessage });
            }

            await this.categoriesService.DeleteCategoryAsync(category);
            string message = new StringBuilder("Category ")
                                                .Append(category.Name)
                                                .Append(GlobalConstants.SuccessfullyDeleteCategoryMessage)
                                                .ToString();

            return this.RedirectToAction("Index", "Categories", new { area = string.Empty, message });
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id, string message = null)
        {
            Category category = await this.categoriesService.GetByIdForEditAsync(id);
            if (category == null)
            {
                this.ViewBag.Message = "No category found";

                return this.View("NotFound");
            }

            EditCategoryViewModel model = AutoMapperConfig.MapperInstance.Map<EditCategoryViewModel>(category);
            this.ViewBag.UserMessage = message;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(EditCategoryViewModel userInputModel)
        {
            Category category = await this.categoriesService.GetByIdForEditAsync(userInputModel.Id);

            if (category == null)
            {
                this.ViewBag.Message = "No category found";
                return this.View("NotFound");
            }

            if (!this.ModelState.IsValid || !this.categoriesService.IsCategoryEdited(category, userInputModel))
            {
                string message = !this.ModelState.IsValid ? GlobalConstants.InvalidDataErrorMessage : GlobalConstants.PleaseMakeYourChangesMessage;

                return this.RedirectToAction("EditCategory", "CategoriesManager", new { id = userInputModel.Id, message });
            }

            if (!this.IsCategoryEdited(userInputModel, category))
            {
                return this.RedirectToAction("EditCategory", "CategoriesManager", new { id = userInputModel.Id, message = GlobalConstants.PleaseMakeYourChangesMessage });
            }

            await this.categoriesService.UpdateCategoryAsync(category, userInputModel);

            return this.RedirectToAction("Index", "Categories", new { area = string.Empty, message = GlobalConstants.SuccessfullyEditedCategoryMessage });
        }

        [HttpGet]
        public async Task<IActionResult> UndeleteCategory(int id)
        {
            Category category = await this.categoriesService.GetDeletedCategoryByIdAsync(id);

            if (category != null)
            {
                await this.categoriesService.UndeleteCategoryAsync(category);
                string message = new StringBuilder("Category ")
                                                .Append(category.Name)
                                                .Append(GlobalConstants.SuccessfullyUndeleteCategoryMessage)
                                                .ToString();

                return this.RedirectToAction("DeletedCategories", new { message });
            }

            this.ViewBag.Message = "No category found";

            return this.View("NotFound");
        }

        private bool IsCategoryEdited(EditCategoryViewModel model, Category cateogry)
        {
            if (cateogry.Name == model.Name && cateogry.ImageURL == model.ImageURL)
            {
                return false;
            }

            return true;
        }
    }
}
