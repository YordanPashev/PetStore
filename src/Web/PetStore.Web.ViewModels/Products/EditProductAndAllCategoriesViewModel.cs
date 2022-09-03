namespace PetStore.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class EditProductAndAllCategoriesViewModel
    {
        public ProductEditViewModel Product { get; set; }

        public ICollection<ListCategoriesOnProductCreateViewModel> Categories { get; set; }
    }
}
