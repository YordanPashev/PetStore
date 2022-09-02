namespace PetStore.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class ProductEditModel
    {
        public ProductModel Product { get; set; }

        public ICollection<ListCategoriesOnProductCreateViewModel> Categories { get; set; }
    }
}
