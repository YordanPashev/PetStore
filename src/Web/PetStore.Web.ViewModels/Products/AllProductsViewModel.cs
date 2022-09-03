namespace PetStore.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class AllProductsViewModel
    {
       public ICollection<ProductDetailsViewModels> AllProducts { get; set; }
    }
}
