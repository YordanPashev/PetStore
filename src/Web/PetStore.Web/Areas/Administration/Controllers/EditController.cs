namespace PetStore.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PetStore.Common;
    using PetStore.Data.Models;
    using PetStore.Services.Data;
    using PetStore.Services.Mapping;
    using PetStore.Web.ViewModels.Categories;
    using PetStore.Web.ViewModels.Products;

    public class EditController : AdministrationController
    {
        private readonly ICategoriesService categoriesService;
        private readonly IProductsService productsService;

        public EditController(ICategoriesService categoriesService, IProductsService productsService)
        {
            this.categoriesService = categoriesService;
            this.productsService = productsService;
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

        [HttpGet]
        public async Task<IActionResult> EditProduct(string id, string message = null)
        {
            Product product = await this.productsService.GetProductByIdForEditAsync(id);
            if (product == null)
            {
                return this.View("NoProductFound");
            }

            EditProductViewModel editPorudctModel = new EditProductViewModel()
            {
                ProductInfo = AutoMapperConfig.MapperInstance.Map<ProductInfoViewModel>(product),
                Categories = this.categoriesService.GetAllCategoriesNoTracking().To<CategoryShortInfoViewModel>().ToArray(),
                UserMessage = message,
            };

            return this.View(editPorudctModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductInfoViewModel userInputModel)
        {
            userInputModel.CategoryId = await this.categoriesService.GetCategoryIdByNameNoTrackingAsync(userInputModel.CategoryName);
            Product product = await this.productsService.GetProductByIdForEditAsync(userInputModel.Id);

            if (!this.ModelState.IsValid || product == null || userInputModel?.CategoryId < 0)
            {
                return this.RedirectToAction("EditProduct", new { id = userInputModel.Id, message = GlobalConstants.InvalidDataErrorMessage });
            }

            if (!this.IsProductEdited(userInputModel, product))
            {
                return this.RedirectToAction("EditProduct", new { id = userInputModel.Id, message = GlobalConstants.EditMessage });
            }

            await this.productsService.UpdateProductDataAsync(userInputModel, product);
            return this.RedirectToAction("Details", "Products", new { area = string.Empty, id = userInputModel.Id, message = GlobalConstants.SuccessfullyEditProductMessage });
        }

        private bool IsProductEdited(ProductInfoViewModel model, Product product)
        {
            if (product.Name == model.Name && product.Price == model.Price && product.Description == model.Description &&
                product.ImageUrl == model.ImageUrl && product.CategoryId == model.CategoryId)
            {
                return false;
            }

            return true;
        }
    }
}
