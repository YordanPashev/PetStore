namespace PetStore.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class ListOfProductsViewModel
    {
        public ListOfProductsViewModel()
            => this.ListOfProducts = new HashSet<ProductShortInfoViewModel>();

        public ICollection<ProductShortInfoViewModel> ListOfProducts { get; set; }

        public string CategoryName { get; set; }

        public string SearchQuery { get; set; }
    }
}
