namespace PetStore.Web.Infrastructures
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Services.Mapping;
    using PetStore.Web.Controllers;
    using PetStore.Web.ViewModels.Categories;
    using PetStore.Web.ViewModels.Products;

    public class CategoriesControllerExtension : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesControllerExtension(ICategoriesService categoriesService)
            => this.categoriesService = categoriesService;

        public IActionResult RedirectOrNotFound(Category category, string productsStatus)
        {
            if (category == null)
            {
                return this.View("NoCategoryFound");
            }

            this.ViewBag.CategoryName = category.Name;
            this.ViewBag.CategoryImageURL = category.ImageURL;

            if (category.Products.Count == 0)
            {
                return this.View("~/Views/Products/Index.cshtml", new ListOfProductsViewModel());
            }

            ListOfProductsViewModel model = new ListOfProductsViewModel()
            {
                ListOfProducts = AutoMapperConfig.MapperInstance.Map<ProductShortInfoViewModel[]>(category.Products),
            };

            if (productsStatus == GlobalConstants.ProductStatusInStock)
            {
                return this.View("~/Views/Products/Index.cshtml", model);
            }

            if (productsStatus == GlobalConstants.ProductStatusDeleted)
            {
                return this.View("~/Views/Products/DeletedProducts.cshtml", model);
            }

            return this.View("Index", "Products");
        }

        public async Task<IActionResult> EditAndRedirectOrReturnMessage(Category category, CategoryProdutsViewModel userInputModel)
        {
            if (!this.categoriesService.IsCategoryEdited(category, userInputModel))
            {
                return this.RedirectToAction("Edit", new { id = userInputModel.Id, message = GlobalConstants.EditMessage });
            }

            await this.categoriesService.UpdateCategoryAsync(category, userInputModel);
            return this.RedirectToAction("Index", "Categories", new { message = GlobalConstants.SuccessfullyEditedCategoryMessage });
        }

        public IActionResult ViewOrNoCategoryFound(IQueryable<Category> allCategories, string message)
        {
            if (allCategories == null)
            {
                return this.View("NoCategoryFound");
            }

            AllCategoriesViewModel categoriesModel = new AllCategoriesViewModel()
            {
                AllCategories = allCategories.To<CategoryProdutsViewModel>().ToList(),
            };

            if (message != null)
            {
                this.ViewBag.UserMessage = message;
            }

            return this.View(categoriesModel);
        }
    }
}
