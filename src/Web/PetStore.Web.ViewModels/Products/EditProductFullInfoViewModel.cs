namespace PetStore.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class EditProductFullInfoViewModel
    {
        public EditProductInfoViewModel Product { get; set; }

        public ICollection<CategoryShortInfoViewModel> Categories { get; set; }

        public string ErrorMessage { get; set; }
    }
}
