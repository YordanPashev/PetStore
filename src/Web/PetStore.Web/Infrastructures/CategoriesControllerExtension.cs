namespace PetStore.Web.Infrastructures
{
    using System.Linq;

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

        public IActionResult RedirectOrNoCategoryFound(Category category, string productsStatus)
        {
            if (category == null)
            {
                this.ViewBag.Message = "No category found";
                return this.View("NotFound");
            }

            this.ViewBag.CategoryImageURL = category.ImageURL;

            if (category.Products.Count == 0)
            {
                return this.View("~/Views/Products/Index.cshtml", new ListOfProductsViewModel() { CategoryName = category.Name });
            }

            ListOfProductsViewModel model = new ListOfProductsViewModel()
            {
                ListOfProducts = AutoMapperConfig.MapperInstance.Map<ProductShortInfoViewModel[]>(category.Products),
                CategoryName = category.Name,
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

        public IActionResult ViewOrNoCategoriesFound(string message)
        {
            CategoriesViewModel categoriesModel = new CategoriesViewModel()
            {
                AllCategories = this.categoriesService.GetAllCategoriesNoTracking().To<CategoryProdutsViewModel>().ToList(),
            };

            this.ViewBag.UserMessage = message;

            return this.View(categoriesModel);
        }
    }
}
