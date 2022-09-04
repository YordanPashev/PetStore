namespace PetStore.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class ListOfProductsViewModel
    {
       public ICollection<ProductDetailsViewModels> AllProducts { get; set; }
    }
}
