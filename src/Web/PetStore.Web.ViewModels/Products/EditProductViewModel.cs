namespace PetStore.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class EditProductViewModel
    {
        public ProductViewModel Product { get; set; }

        public ICollection<ListCategoriesOnProductCreateViewModel> Categories { get; set; }
    }
}
