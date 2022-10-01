namespace PetStore.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    public class AllCategoriesViewModel
    {
        public AllCategoriesViewModel()
            => this.AllCategories = new HashSet<CategoryProdutsViewModel>();

        public ICollection<CategoryProdutsViewModel> AllCategories { get; set; }
    }
}
