namespace PetStore.Web.Infrastructures
{
    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Mapping;
    using PetStore.Web.Controllers;
    using PetStore.Web.ViewModels.Products;

    public class CategoriesControllerExtension : BaseController
    {
        public IActionResult RedirectOrNotFound(Category category, string productsStatus)
        {
            if (category == null)
            {
                return this.View("NoCategoryFound");
            }

            if (category.Products.Count == 0)
            {
                return this.View("NoProductFound");
            }

            ListOfProductsViewModel model = new ListOfProductsViewModel()
            {
                ListOfProducts = AutoMapperConfig.MapperInstance.Map<ProductShortInfoViewModel[]>(category.Products),
            };
            this.ViewBag.CategoryName = category.Name;

            if (productsStatus == GlobalConstants.ProductStatusInStock)
            {
                return this.View("~/Views/Products/Index.cshtml", model);
            }

            if (productsStatus == GlobalConstants.ProductStatusDeleted)
            {
                return this.View("~/Views/Products/DeletedProducts.cshtml", model);
            }

            return this.View("NoProductsFound");
        }
    }
}
