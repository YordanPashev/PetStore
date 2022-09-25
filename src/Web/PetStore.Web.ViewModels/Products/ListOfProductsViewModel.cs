namespace PetStore.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class ListOfProductsViewModel
    {
       public ICollection<ProductShortInfoViewModel> ListOfProducts { get; set; }

       public string SearchQuery { get; set; }
    }
}
