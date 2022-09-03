namespace PetStore.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class AllProductsViewModel
    {
       public ICollection<ProductViewModel> AllProducts { get; set; }
    }
}
