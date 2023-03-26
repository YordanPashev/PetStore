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
