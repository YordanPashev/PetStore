namespace PetStore.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    public class CategoriesViewModel
    {
        public CategoriesViewModel()
            => this.AllCategories = new HashSet<CategoryProdutsViewModel>();

        public ICollection<CategoryProdutsViewModel> AllCategories { get; set; }
    }
}
