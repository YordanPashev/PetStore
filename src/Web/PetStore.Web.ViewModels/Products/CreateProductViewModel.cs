namespace PetStore.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class CreateProductViewModel
    {
        public ICollection<CategoryShortInfoViewModel> CategoriesIfo { get; set; }

        public string ErrorMessage { get; set; }
    }
}
